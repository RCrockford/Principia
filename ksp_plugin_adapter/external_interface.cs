﻿using System;
using System.Collections.Generic;
using System.Security.Authentication;

namespace principia {
namespace ksp_plugin_adapter {

public class ExternalInterface {
  public XY GeopotentialGetCoefficient(int body_index, int degree, int order) {
    ThrowOnError(
        adapter_.Plugin().ExternalGeopotentialGetCoefficient(
            body_index, degree, order, out XY result));
    return result;
  }

  public double GeopotentialGetReferenceRadius(int body_index) {
    ThrowOnError(
        adapter_.Plugin().ExternalGeopotentialGetReferenceRadius(
            body_index, out double result));
    return result;
  }

  public bool FlightPlanExists(string vessel_guid) {
    ThrowOnError(
        adapter_.Plugin().FlightPlanExists(
            vessel_guid, out bool result));
    return result;
  }

  public int FlightPlanNumberOfManoeuvres(string vessel_guid) {
    ThrowOnError(
        adapter_.Plugin().FlightPlanNumberOfManoeuvres(
            vessel_guid, out int result));
    return result;
  }

  public double FlightPlanGetManoeuvreInitialTime(string vessel_guid, int index)
  {
    ThrowOnError(
        adapter_.Plugin().FlightPlanExists(
            vessel_guid, out bool plan_exists));
    if (plan_exists)
    {
      ThrowOnError(
          adapter_.Plugin().FlightPlanGetManoeuvre(
              vessel_guid, index, out NavigationManoeuvre manoeuvre));

      return ToGameTime(manoeuvre.burn.initial_time);
    }
    throw new ArgumentOutOfRangeException("OUT_OF_RANGE");
  }

  public double FlightPlanGetManoeuvreDeltaV(string vessel_guid)
  {
    ThrowOnError(
        adapter_.Plugin().FlightPlanExists(
            vessel_guid, out bool plan_exists));
    if (plan_exists)
    {
      ThrowOnError(
          adapter_.Plugin().FlightPlanGetManoeuvre(
              vessel_guid, index, out NavigationManoeuvre manoeuvre));

      return sqrt(manoeuvre.burn.delta_v.x * manoeuvre.burn.delta_v.x +
                  manoeuvre.burn.delta_v.y * manoeuvre.burn.delta_v.y +
                  manoeuvre.burn.delta_v.z * manoeuvre.burn.delta_v.z);
    }
    throw new ArgumentOutOfRangeException("OUT_OF_RANGE");
  }

  public double FlightPlanGetManoeuvreDuration(string vessel_guid)
  {
    ThrowOnError(
        adapter_.Plugin().FlightPlanExists(
            vessel_guid, out bool plan_exists));
    if (plan_exists)
    {
      ThrowOnError(
          adapter_.Plugin().FlightPlanGetManoeuvre(
              vessel_guid, index, out NavigationManoeuvre manoeuvre));

      return ToGameTime(manoeuvre.duration);
    }
    throw new ArgumentOutOfRangeException("OUT_OF_RANGE");
  }

  public XYZ FlightPlanGetManoeuvreGuidance(string vessel_guid)
  {
    ThrowOnError(
        adapter_.Plugin().FlightPlanExists(
            vessel_guid, out bool plan_exists));
    if (plan_exists)
    {
      ThrowOnError(
          adapter_.Plugin().FlightPlanGetGuidance(
              vessel_guid, index, out XYZ result));

      return result;
    }
    throw new ArgumentOutOfRangeException("OUT_OF_RANGE");
  }

  public static ExternalInterface Get() {
    List<ScenarioModule> modules;
    try {
      modules = ScenarioRunner.GetLoadedModules();
    } catch {
      return null;
    }
    foreach (var module in modules) {
      if (module is PrincipiaPluginAdapter) {
        return new ExternalInterface((PrincipiaPluginAdapter)module);
      }
    }
    return null;
  }

  private static void ThrowOnError(Status status) {
    switch (status.error) {
      case 0: return;
      case 1: throw new OperationCanceledException("CANCELLED");
      case 2: throw new Exception("UNKNOWN");
      case 3: throw new ArgumentException("INVALID_ARGUMENT");
      case 4: throw new TimeoutException("DEADLINE_EXCEEDED");
      case 5: throw new KeyNotFoundException("NOT_FOUND");
      case 6: throw new ArgumentException("ALREADY_EXISTS");
      case 7: throw new UnauthorizedAccessException("PERMISSION_DENIED");
      case 16: throw new AuthenticationException("UNAUTHENTICATED");
      case 8: throw new Exception("RESOURCE_EXHAUSTED");
      case 9: throw new Exception("FAILED_PRECONDITION");
      case 10: throw new Exception("ABORTED");
      case 11: throw new ArgumentOutOfRangeException("OUT_OF_RANGE");
      case 12: throw new NotImplementedException("UNIMPLEMENTED");
      case 13: throw new Exception("INTERNAL");
      case 14: throw new Exception("UNAVAILABLE");
      case 15: throw new Exception("DATA_LOSS");
      default: throw new Exception($"Error {status.error}");
    }
  }

  private ExternalInterface(PrincipiaPluginAdapter adapter) {
    adapter_ = adapter;
    if (!Loader.loaded_principia_dll_) {
      throw new DllNotFoundException(
          "The Principia native DLL failed to load");
    }
  }

  private readonly PrincipiaPluginAdapter adapter_;
}

}  // namespace ksp_plugin_adapter
}  // namespace principia
