
#include <string>

#include "astronomy/solar_system_fingerprints.hpp"
#include "astronomy/stabilize_ksp.hpp"
#include "base/fingerprint2011.hpp"
#include "geometry/frame.hpp"
#include "glog/logging.h"
#include "gmock/gmock.h"
#include "gtest/gtest.h"
#include "physics/solar_system.hpp"
#include "serialization/geometry.pb.h"
#include "serialization/physics.pb.h"

namespace principia {
namespace astronomy {

using base::Fingerprint2011;
using geometry::Frame;
using physics::SolarSystem;
using ::testing::Eq;

class KSPFingerprintTest : public ::testing::Test {
 protected:
  using Barycentric = Frame<serialization::Frame::PluginTag,
                            serialization::Frame::BARYCENTRIC,
                            true>;

  KSPFingerprintTest()
      : solar_system_(
            SOLUTION_DIR / "astronomy" / "kerbol_gravity_model.proto.txt",
            SOLUTION_DIR / "astronomy" / "kerbol_initial_state_0_0.proto.txt") {
    google::LogToStderr();
  }

  SolarSystem<Barycentric> solar_system_;
};

TEST_F(KSPFingerprintTest, Stock) {
  auto const hierarchical_system = solar_system_.MakeHierarchicalSystem();
  serialization::HierarchicalSystem message;
  hierarchical_system->WriteToMessage(&message);
  std::string const serialized_message = message.SerializeAsString();
  uint64_t const fingerprint = Fingerprint2011(serialized_message.c_str(),
                               serialized_message.size());
  LOG(INFO) << "Stock KSP fingerprint is 0x" << std::hex << std::uppercase
            << fingerprint;
  EXPECT_THAT(fingerprint, Eq(KSPStockSystemFingerprint));
}

TEST_F(KSPFingerprintTest, Corrected) {
  StabilizeKSP(solar_system_);
  auto const hierarchical_system = solar_system_.MakeHierarchicalSystem();
  serialization::HierarchicalSystem message;
  hierarchical_system->WriteToMessage(&message);
  std::string const serialized_message = message.SerializeAsString();
  uint64_t const fingerprint = Fingerprint2011(serialized_message.c_str(),
                               serialized_message.size());
  LOG(INFO) << "Corrected KSP fingerprint is 0x" << std::hex << std::uppercase
            << fingerprint;
  EXPECT_THAT(fingerprint, Eq(KSPStabilizedSystemFingerprint));
}

}  // namespace astronomy
}  // namespace principia
