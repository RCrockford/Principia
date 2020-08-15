
#pragma once

#include <initializer_list>
#include <vector>

#include "base/tags.hpp"

namespace principia {
namespace numerics {
namespace internal_unbounded_arrays {

using base::uninitialized_t;

// An allocator that does not initialize the allocated objects.
template<class T>
class uninitialized_allocator : public std::allocator<T> {
 public:
  template<class U, class... Args>
  void construct(U* p, Args&&... args);
};

// The following classes are similar to those in fixed_arrays.hpp, but they have
// an Extend method to add more entries to the arrays.

template<typename Scalar>
class UnboundedVector final {
 public:
  UnboundedVector(int size);
  UnboundedVector(int size, uninitialized_t);
  UnboundedVector(std::initializer_list<Scalar> data);

  void Extend(int extra_size);
  void Extend(int extra_size, uninitialized_t);
  void Extend(std::initializer_list<Scalar> data);

  int size() const;

  bool operator==(UnboundedVector const& right) const;

  Scalar& operator[](int index);
  Scalar const& operator[](int index) const;

 private:
  std::vector<Scalar, uninitialized_allocator<Scalar>> data_;
};

template<typename Scalar>
class UnboundedLowerTriangularMatrix final {
 public:
  UnboundedLowerTriangularMatrix(int rows);
  UnboundedLowerTriangularMatrix(int rows, uninitialized_t);

  // The |data| must be in row-major format.
  UnboundedLowerTriangularMatrix(std::initializer_list<Scalar> data);

  void Extend(int extra_rows);
  void Extend(int extra_rows, uninitialized_t);

  // The |data| must be in row-major format.
  void Extend(std::initializer_list<Scalar> data);

  int rows() const;
  int dimension() const;

  bool operator==(UnboundedLowerTriangularMatrix const& right) const;

  // For  0 < j <= i < rows, the entry a_ij is accessed as |a[i][j]|.
  // if i and j do not satisfy these conditions, the expression |a[i][j]| is
  // erroneous.
  Scalar* operator[](int index);
  Scalar const* operator[](int index) const;

 private:
  int rows_;
  std::vector<Scalar, uninitialized_allocator<Scalar>> data_;
};

}  // namespace internal_unbounded_arrays

using internal_unbounded_arrays::UnboundedLowerTriangularMatrix;
using internal_unbounded_arrays::UnboundedVector;

}  // namespace numerics
}  // namespace principia

#include "numerics/unbounded_arrays_body.hpp"
