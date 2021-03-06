﻿
#pragma once

#include "mathematica/mathematica.hpp"

#include <cmath>
#include <map>
#include <string>
#include <tuple>
#include <vector>

#include "base/not_constructible.hpp"
#include "base/traits.hpp"
#include "quantities/si.hpp"

namespace principia {
namespace mathematica {
namespace internal_mathematica {

using astronomy::J2000;
using base::is_instance_of_v;
using base::not_constructible;
using base::not_null;
using quantities::DebugString;
using quantities::IsFinite;
using quantities::si::Metre;
using quantities::si::Radian;
using quantities::si::Second;
namespace si = quantities::si;

// Wraps the string in quotes and escapes things properly.
inline std::string Escape(std::string_view const str) {
  std::string result = "\"";
  for (const char c : str) {
    switch (c) {
      case '"':
        result += "\\\"";
        break;
      case '\\':
        result += "\\\\";
        break;
      default:
        result += c;
        break;
    }
  }
  result += "\"";
  return result;
}

// Does not wrap its arguments in ToMathematica.
inline std::string Apply(
    std::string const& function,
    std::vector<std::string> const& arguments) {
  std::string result;
  result += function;
  result += "[";
  for (int i = 0; i < arguments.size(); ++i) {
    result += arguments[i];
    result += (i + 1 < arguments.size() ? "," : "");
  }
  result += "]";
  return result;
}

// A helper struct to scan the elements of a tuple and stringify them.
template<int index, typename Tuple, typename OptionalExpressIn>
struct TupleHelper : not_constructible {
  static void ToMathematicaStrings(Tuple const& tuple,
                                   std::vector<std::string>& expressions,
                                   OptionalExpressIn express_in) {
    TupleHelper<index - 1, Tuple, OptionalExpressIn>::ToMathematicaStrings(
        tuple, expressions, express_in);
    expressions.push_back(ToMathematica(std::get<index - 1>(tuple),
                          express_in));
  }
};

template<typename Tuple, typename OptionalExpressIn>
struct TupleHelper<0, Tuple, OptionalExpressIn> : not_constructible {
  static void ToMathematicaStrings(Tuple const& tuple,
                                   std::vector<std::string>& expressions,
                                   OptionalExpressIn express_in) {}
};

template<typename V, typename A, int d,
         template<typename, typename, int> class E,
         typename OptionalExpressIn>
std::string ToMathematicaExpression(
    PolynomialInMonomialBasis<V, A, d, E> const& polynomial,
    OptionalExpressIn express_in) {
  using Coefficients =
      typename PolynomialInMonomialBasis<V, A, d, E>::Coefficients;
  std::vector<std::string> coefficients;
  coefficients.reserve(std::tuple_size_v<Coefficients>);
  TupleHelper<std::tuple_size_v<Coefficients>,
              Coefficients,
              OptionalExpressIn>::ToMathematicaStrings(polynomial.coefficients_,
                                                       coefficients,
                                                       express_in);
  std::string argument;
  if constexpr (is_instance_of_v<Point, A>) {
    argument =
        Apply("Subtract", {"#", ToMathematica(polynomial.origin_, express_in)});
  } else {
    argument = "#";
  }
  std::vector<std::string> monomials;
  for (int i = 0; i < coefficients.size(); ++i) {
    if (i == 0) {
      monomials.push_back(coefficients[i]);
    } else if (i == 1) {
      monomials.push_back(Apply("Times", {coefficients[i], argument}));
    } else {
      monomials.push_back(Apply(
          "Times",
          {coefficients[i], Apply("Power", {argument, std::to_string(i)})}));
    }
  }
  return Apply("Plus", monomials);
}

template<typename V, int ad, int pd,
         template<typename, typename, int> class E,
         typename OptionalExpressIn>
std::string ToMathematicaExpression(PoissonSeries<V, ad, pd, E> const& series,
                                    OptionalExpressIn express_in) {
  std::vector<std::string> components = {
      ToMathematicaExpression(series.aperiodic_, express_in)};
  for (auto const& [ω, polynomials] : series.periodic_) {
    std::string const polynomial_sin =
        ToMathematicaExpression(polynomials.sin, express_in);
    std::string const polynomial_cos =
        ToMathematicaExpression(polynomials.cos, express_in);
    std::string const angle =
        Apply("Times",
              {ToMathematica(ω, express_in),
               Apply("Subtract",
                     {"#", ToMathematica(series.origin_, express_in)})});
    components.push_back(Apply("Times",
                               {polynomial_sin, Apply("Sin", {angle})}));
    components.push_back(Apply("Times",
                               {polynomial_cos, Apply("Cos", {angle})}));
  }
  return Apply("Plus", components);
}

template<typename V, int ad, int pd,
         template<typename, typename, int> class E,
         typename OptionalExpressIn>
std::string ToMathematicaExpression(
    PiecewisePoissonSeries<V, ad, pd, E> const& series,
    OptionalExpressIn express_in) {
  std::vector<std::string> conditions_and_functions;
  for (int i = 0; i < series.series_.size(); ++i) {
    std::string const function =
        ToMathematicaExpression(series.series_[i], express_in);
    std::string const condition =
        Apply("Between",
              {"#",
               Apply("List",
                     {ToMathematica(series.bounds_[i], express_in),
                      ToMathematica(series.bounds_[i + 1], express_in)})});
    conditions_and_functions.push_back(Apply("List", {function, condition}));
  }
  return Apply("Piecewise", {Apply("List", conditions_and_functions)});
}

template<typename... Qs>
ExpressIn<Qs...>::ExpressIn(Qs const&... qs)
    : units_(std::make_tuple(qs...)) {}

template<typename... Qs>
template<typename Q>
double ExpressIn<Qs...>::operator()(Q const& q) const {
  return Divide<Q::Dimensions::Length, Length>(
      Divide<Q::Dimensions::Mass, Mass>(
          Divide<Q::Dimensions::Time, Time>(
              Divide<Q::Dimensions::Current, Current>(
                  Divide<Q::Dimensions::Temperature, Temperature>(
                      Divide<Q::Dimensions::Amount, Amount>(
                          Divide<Q::Dimensions::LuminousIntensity,
                                 LuminousIntensity>(
                              Divide<Q::Dimensions::Angle, Angle>(q))))))));
}

template<typename... Qs>
template<std::int64_t exponent, typename Q1, typename Q2>
Quotient<Q2, Exponentiation<Q1, exponent>> ExpressIn<Qs...>::Divide(
    Q2 const& q2) const {
  if constexpr (exponent == 0) {
    return q2;
  } else {
    return q2 / Pow<exponent>(std::get<Q1>(units_));
  }
}

template<typename T, typename OptionalExpressIn>
std::string Option(std::string const& name,
                   T const& right,
                   OptionalExpressIn express_in) {
  return Apply("Rule", {name, ToMathematica(right, express_in)});
}

template<typename T, typename OptionalExpressIn>
std::string Assign(std::string const& name,
                   T const& right,
                   OptionalExpressIn express_in) {
  return Apply("Set", {name, ToMathematica(right, express_in)}) + ";\n";
}

template<typename T, typename U, typename OptionalExpressIn>
std::string PlottableDataset(std::vector<T> const& x,
                             std::vector<U> const& y,
                             OptionalExpressIn express_in) {
  std::vector<std::string> const xy = {ToMathematica(x, express_in),
                                       ToMathematica(y, express_in)};
  return Apply("Transpose", {Apply("List", xy)});
}

template<typename T, typename OptionalExpressIn>
std::string ToMathematica(std::vector<T> const& list,
                          OptionalExpressIn express_in) {
  std::vector<std::string> expressions;
  expressions.reserve(list.size());
  for (auto const& expression : list) {
    expressions.emplace_back(ToMathematica(expression, express_in));
  }
  return Apply("List", expressions);
}

template<typename It, typename OptionalExpressIn>
std::string ToMathematica(It const begin, It const end,
                          OptionalExpressIn express_in) {
  std::vector<std::string> expressions;
  for (auto it = begin; it != end; ++it) {
    expressions.emplace_back(ToMathematica(*it, express_in));
  }
  return Apply("List", expressions);
}

template<typename OptionalExpressIn>
std::string ToMathematica(bool const b, OptionalExpressIn /*express_in*/) {
  return b ? "True" : "False";
}

template<typename T, typename, typename OptionalExpressIn>
std::string ToMathematica(T const integer, OptionalExpressIn /*express_in*/) {
  return std::to_string(integer);
}

template<typename T, typename, typename OptionalExpressIn, typename>
std::string ToMathematica(T const real,
                          OptionalExpressIn /*express_in*/) {
  if (std::isinf(real)) {
    if (real > 0.0) {
      return "Infinity";
    } else {
      return Apply("Minus", {"Infinity"});
    }
  } else if (std::isnan(real)) {
    return "Indeterminate";
  } else {
    std::string s = DebugString(real);
    s.replace(s.find("e"), 1, "*^");
    return Apply("SetPrecision", {s, "$MachinePrecision"});
  }
}

template<typename OptionalExpressIn>
std::string ToMathematica(Quaternion const& quaternion,
                          OptionalExpressIn /*express_in*/) {
  return Apply("Quaternion",
               {ToMathematica(quaternion.real_part()),
                ToMathematica(quaternion.imaginary_part().x),
                ToMathematica(quaternion.imaginary_part().y),
                ToMathematica(quaternion.imaginary_part().z)});
}

template<typename T, int size, typename OptionalExpressIn>
std::string ToMathematica(FixedVector<T, size> const& fixed_vector,
                          OptionalExpressIn express_in) {
  std::vector<std::string> expressions;
  for (int i = 0; i < size; ++i) {
    expressions.emplace_back(ToMathematica(fixed_vector[i], express_in));
  }
  return Apply("List", expressions);
}

template<typename T, typename OptionalExpressIn>
std::string ToMathematica(R3Element<T> const& r3_element,
                          OptionalExpressIn express_in) {
  std::vector<std::string> expressions;
  expressions.emplace_back(ToMathematica(r3_element.x, express_in));
  expressions.emplace_back(ToMathematica(r3_element.y, express_in));
  expressions.emplace_back(ToMathematica(r3_element.z, express_in));
  return Apply("List", expressions);
}

template<typename T, typename OptionalExpressIn>
std::string ToMathematica(R3x3Matrix<T> const& r3x3_matrix,
                          OptionalExpressIn express_in) {
  std::vector<R3Element<T>> rows;
  rows.push_back(r3x3_matrix.row_x());
  rows.push_back(r3x3_matrix.row_y());
  rows.push_back(r3x3_matrix.row_z());
  return ToMathematica(rows, express_in);
}

template<typename D, typename... Qs>
std::string ToMathematica(Quantity<D> const& quantity,
                          ExpressIn<Qs...> express_in) {
  return ToMathematica(express_in(quantity));
}

template<typename D, typename OptionalExpressIn>
std::string ToMathematica(Quantity<D> const& quantity,
                          std::nullopt_t express_in) {
  std::string s = DebugString(quantity);
  std::string const number = ToMathematica(quantity / si::Unit<Quantity<D>>);
  std::size_t const split = s.find(" ");
  std::string const units = Escape(s.substr(split, s.size()));
  return Apply("Quantity", {number, units});
}

template<typename S, typename F, typename OptionalExpressIn>
std::string ToMathematica(Vector<S, F> const& vector,
                          OptionalExpressIn express_in) {
  return ToMathematica(vector.coordinates(), express_in);
}

template<typename S, typename F, typename OptionalExpressIn>
std::string ToMathematica(Bivector<S, F> const& bivector,
                          OptionalExpressIn express_in) {
  return ToMathematica(bivector.coordinates(), express_in);
}

template<typename V, typename OptionalExpressIn>
std::string ToMathematica(Point<V> const & point,
                          OptionalExpressIn express_in) {
  return ToMathematica(point - Point<V>(), express_in);
}

template<typename S, typename F,
         template<typename, typename> typename M,
         typename OptionalExpressIn>
std::string ToMathematica(SymmetricBilinearForm<S, F, M> const& form,
                          OptionalExpressIn express_in) {
  return ToMathematica(form.coordinates(), express_in);
}

template<typename F, typename OptionalExpressIn>
std::string ToMathematica(DegreesOfFreedom<F> const& degrees_of_freedom,
                          OptionalExpressIn express_in) {
  return Apply(
      "List",
      std::vector<std::string>{
          ToMathematica(degrees_of_freedom.position(), express_in),
          ToMathematica(degrees_of_freedom.velocity(), express_in)});
}

template<typename Tuple, typename, typename OptionalExpressIn>
std::string ToMathematica(Tuple const& tuple, OptionalExpressIn express_in) {
  std::vector<std::string> expressions;
  expressions.reserve(std::tuple_size_v<Tuple>);
  TupleHelper<std::tuple_size_v<Tuple>, Tuple, OptionalExpressIn>::
      ToMathematicaStrings(tuple, expressions, express_in);
  return Apply("List", expressions);
}

template<typename R, typename, typename, typename OptionalExpressIn>
std::string ToMathematica(R const ref,
                          OptionalExpressIn express_in) {
  return Apply("List",
               std::vector<std::string>{
                   ToMathematica(ref.time, express_in),
                   ToMathematica(ref.degrees_of_freedom, express_in)});
}

template<typename V, typename A, int d,
         template<typename, typename, int> class E,
         typename OptionalExpressIn>
std::string ToMathematica(
    PolynomialInMonomialBasis<V, A, d, E> const& polynomial,
    OptionalExpressIn express_in) {
  return Apply("Function", {ToMathematicaExpression(polynomial, express_in)});
}

template<typename V, int ad, int pd,
         template<typename, typename, int> class E,
         typename OptionalExpressIn>
std::string ToMathematica(PoissonSeries<V, ad, pd, E> const& series,
                          OptionalExpressIn express_in) {
  return Apply("Function", {ToMathematicaExpression(series, express_in)});
}

template<typename V, int ad, int pd,
         template<typename, typename, int> class E,
         typename OptionalExpressIn>
std::string ToMathematica(PiecewisePoissonSeries<V, ad, pd, E> const& series,
                          OptionalExpressIn express_in) {
  return Apply("Function", {ToMathematicaExpression(series, express_in)});
}

template<typename OptionalExpressIn>
std::string ToMathematica(
    astronomy::OrbitalElements::EquinoctialElements const& elements,
    OptionalExpressIn express_in) {
  return ToMathematica(std::make_tuple((elements.t - J2000),
                                       elements.a,
                                       elements.h,
                                       elements.k,
                                       elements.λ,
                                       elements.p,
                                       elements.q,
                                       elements.pʹ,
                                       elements.qʹ),
                       express_in);
}

template<typename T, typename OptionalExpressIn>
std::string ToMathematica(std::optional<T> const& opt,
                          OptionalExpressIn express_in) {
  std::vector<T> value;
  if (opt.has_value()) {
    value.push_back(opt.value());
  }
  return ToMathematica(value, express_in);
}

template<typename OptionalExpressIn>
std::string ToMathematica(char const* const str,
                          OptionalExpressIn /*express_in*/) {
  return Escape(str);
}

template<typename OptionalExpressIn>
std::string ToMathematica(std::string const& str,
                          OptionalExpressIn /*express_in*/) {
  return Escape(str);
}

inline Logger::Logger(std::filesystem::path const& path, bool const make_unique)
    : file_([make_unique, &path]() {
        if (make_unique || PRINCIPIA_MATHEMATICA_LOGGER_REGRESSION_TEST != 0) {
          std::filesystem::path filename = path.stem();
          if (make_unique) {
            filename += std::to_string(id_++);
          }
#if PRINCIPIA_MATHEMATICA_LOGGER_REGRESSION_TEST
          filename += "_new";
#endif
          filename += path.extension();
          return path.parent_path() / filename;
        } else {
          return path;
        }
      }()) {}

inline Logger::~Logger() {
  for (auto const& [name, values] : name_and_multiple_values_) {
    file_ << Apply("Set", {name, Apply("List", values)}) + ";\n";
  }
  for (auto const& [name, value] : name_and_single_value_) {
    file_ << Apply("Set", {name, value}) + ";\n";
  }
}

template<typename... Args>
void Logger::Append(std::string const& name, Args... args) {
  name_and_multiple_values_[name].push_back(ToMathematica(args...));
}

template<typename... Args>
void Logger::Set(std::string const& name, Args... args) {
  name_and_single_value_[name] = ToMathematica(args...);
}

inline std::atomic_uint64_t Logger::id_ = 0;

}  // namespace internal_mathematica
}  // namespace mathematica
}  // namespace principia
