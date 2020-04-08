﻿using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace principia {
namespace ksp_plugin_adapter {

internal static partial class Interface {
  private static IntPtr At(this IntPtr pointer, long offset) {
    return new IntPtr(pointer.ToInt64() + offset);
  }

  internal class InBodyParametersMarshaler : MonoMarshaler {

    [StructLayout(LayoutKind.Sequential)]
    internal class BodyParametersRepresentation {
      public string name;
      public string gravitational_parameter;
      public string reference_instant;
      public string min_radius;
      public string mean_radius;
      public string max_radius;
      public string axis_right_ascension;
      public string axis_declination;
      public string reference_angle;
      public string angular_frequency;
      public string reference_radius;
      public string j2;
      public IntPtr geopotential;
      public int geopotential_size;
    }

    public static ICustomMarshaler GetInstance(string s) {
      return instance_;
    }

    public override void CleanUpNativeDataImplementation(IntPtr native_data) {
      var representation = new BodyParametersRepresentation();
      Marshal.PtrToStructure(native_data, representation);
      for (int i = 0; i < representation.geopotential_size; ++i) {
        Marshal.DestroyStructure(
            representation.geopotential.At(
                i * Marshal.SizeOf(typeof(BodyGeopotentialElement))),
            typeof(BodyGeopotentialElement));
      }
      Marshal.FreeHGlobal(representation.geopotential);
      Marshal.FreeHGlobal(native_data);
    }

    public override IntPtr MarshalManagedToNativeImplementation(
        object managed_object) {
      var parameters = managed_object as BodyParameters;
      Debug.Assert(parameters != null, nameof(parameters) + " != null");
      var representation = new BodyParametersRepresentation{
          angular_frequency       = parameters.angular_frequency,
          axis_declination        = parameters.axis_declination,
          axis_right_ascension    = parameters.axis_right_ascension,
          gravitational_parameter = parameters.gravitational_parameter,
          j2                      = parameters.j2,
          max_radius              = parameters.max_radius,
          mean_radius             = parameters.mean_radius,
          min_radius              = parameters.min_radius,
          name                    = parameters.name,
          reference_angle         = parameters.reference_angle,
          reference_instant       = parameters.reference_instant,
          reference_radius        = parameters.reference_radius,
          geopotential_size       = parameters.geopotential?.Length ?? 0
      };
      if (representation.geopotential_size == 0) {
        representation.geopotential = IntPtr.Zero;
      } else {
        int sizeof_element = Marshal.SizeOf(typeof(BodyGeopotentialElement));
        representation.geopotential = Marshal.AllocHGlobal(
            sizeof_element * parameters.geopotential.Length);
        for (int i = 0; i < parameters.geopotential.Length; ++i) {
          Marshal.StructureToPtr(
              parameters.geopotential[i],
              representation.geopotential.At(i * sizeof_element),
              fDeleteOld: false);
        }
      }
      IntPtr buffer = Marshal.AllocHGlobal(Marshal.SizeOf(representation));
      Marshal.StructureToPtr(representation, buffer, fDeleteOld: false);
      return buffer;
    }

    public override object MarshalNativeToManaged(IntPtr native_data) {
      throw Log.Fatal("InBodyParametersMarshaler.MarshalNativeToManaged");
    }

    private static readonly InBodyParametersMarshaler instance_ =
        new InBodyParametersMarshaler();
  }

}

internal class ConfigurationAccuracyParametersMarshaler : MonoMarshaler {
  [StructLayout(LayoutKind.Sequential)]
  internal struct Representation {
    public IntPtr fitting_tolerance;
    public IntPtr geopotential_tolerance;
  }

  public static ICustomMarshaler GetInstance(string s) {
    return instance_;
  }

  public override void CleanUpNativeDataImplementation(IntPtr native_data) {
    var representation = new Representation();
    Marshal.PtrToStructure(native_data, representation);
    NoOwnershipTransferUTF8Marshaler.GetInstance("").
        CleanUpNativeData(representation.fitting_tolerance);
    NoOwnershipTransferUTF8Marshaler.GetInstance("").
        CleanUpNativeData(representation.geopotential_tolerance);
    Marshal.FreeHGlobal(native_data);
  }

  public override IntPtr MarshalManagedToNativeImplementation(
      object managed_object) {
    if (!(managed_object is ConfigurationAccuracyParameters value)) {
      throw new NotSupportedException();
    }
    var representation = new Representation{
        fitting_tolerance = NoOwnershipTransferUTF8Marshaler.GetInstance("").
            MarshalManagedToNative(value.fitting_tolerance),
        geopotential_tolerance = NoOwnershipTransferUTF8Marshaler.
            GetInstance("").
            MarshalManagedToNative(value.geopotential_tolerance),
    };
    IntPtr buffer = Marshal.AllocHGlobal(Marshal.SizeOf(representation));
    Marshal.StructureToPtr(representation, buffer, fDeleteOld: false);
    return buffer;
  }

  public override object MarshalNativeToManaged(IntPtr native_data) {
    var representation = new Representation();
    Marshal.PtrToStructure(native_data, representation);

    return new ConfigurationAccuracyParameters{
        fitting_tolerance = NoOwnershipTransferUTF8Marshaler.GetInstance("").
                                MarshalNativeToManaged(
                                    representation.fitting_tolerance) as String,
        geopotential_tolerance = NoOwnershipTransferUTF8Marshaler.
                                         GetInstance("").
                                         MarshalNativeToManaged(
                                             representation.
                                                 geopotential_tolerance)
                                     as String,
    };
  }

  private static readonly ConfigurationAccuracyParametersMarshaler instance_ =
      new ConfigurationAccuracyParametersMarshaler();
}

}  // namespace ksp_plugin_adapter
}  // namespace principia
