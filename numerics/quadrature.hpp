﻿
#pragma once

#include <optional>
#include <type_traits>

#include "quantities/named_quantities.hpp"

namespace principia {
namespace numerics {
namespace quadrature {
namespace internal_quadrature {

using quantities::Primitive;

template<int points, typename Argument, typename Function>
Primitive<std::invoke_result_t<Function, Argument>, Argument> GaussLegendre(
    Function const& f,
    Argument const& lower_bound,
    Argument const& upper_bound);

// Computes a Clenshaw-Curtis quadrature on 2ᵖ + 1 points for successive p until
// the tolerance is satisfied.  The client controls the accuracy of the result
// using |max_relative_error| (returns when the relative error on the result is
// estimated to be less that the specified value) and |max_points| (returns when
// the number of points would exceed the specified value).
template<int initial_points = 2, typename Argument, typename Function>
Primitive<std::invoke_result_t<Function, Argument>, Argument>
AutomaticClenshawCurtis(
    Function const& f,
    Argument const& lower_bound,
    Argument const& upper_bound,
    std::optional<double> max_relative_error,
    std::optional<int> max_points);

// |points| must be of the form 2ᵖ + 1 for some p ∈ ℕ.  Returns the
// Clenshaw-Curtis quadrature of f with the given number of points.
template<int points, typename Argument, typename Function>
Primitive<std::invoke_result_t<Function, Argument>, Argument> ClenshawCurtis(
    Function const& f,
    Argument const& lower_bound,
    Argument const& upper_bound);

template<typename Argument, typename Function>
Primitive<std::invoke_result_t<Function, Argument>, Argument> Midpoint(
    Function const& f,
    Argument const& lower_bound,
    Argument const& upper_bound,
    int intervals);

}  // namespace internal_quadrature

using internal_quadrature::AutomaticClenshawCurtis;
using internal_quadrature::GaussLegendre;
using internal_quadrature::Midpoint;

}  // namespace quadrature
}  // namespace numerics
}  // namespace principia

#include "numerics/quadrature_body.hpp"