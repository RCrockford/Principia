// Warning!  This file was generated by running a program (see project |tools|).
// If you change it, the changes will be lost the next time the generator is
// run.  You should change the generator instead.

using System;
using System.Runtime.InteropServices;

namespace principia {
namespace ksp_plugin_adapter {

internal class BodyGeopotentialElementMarshaler : MonoMarshaler {
  [StructLayout(LayoutKind.Sequential)]
  internal struct Representation {
    public IntPtr degree;
    public IntPtr order;
    public IntPtr cos;
    public IntPtr j;
    public IntPtr sin;
  }

  public static ICustomMarshaler GetInstance(string s) {
    return instance_;
  }

  public override void CleanUpNativeDataImplementation(IntPtr native_data) {
    var representation = (Representation)Marshal.PtrToStructure(native_data, typeof(Representation));
    NoOwnershipTransferUTF8Marshaler.GetInstance(null).CleanUpNativeData(representation.degree);
    NoOwnershipTransferUTF8Marshaler.GetInstance(null).CleanUpNativeData(representation.order);
    NoOwnershipTransferUTF8Marshaler.GetInstance(null).CleanUpNativeData(representation.cos);
    NoOwnershipTransferUTF8Marshaler.GetInstance(null).CleanUpNativeData(representation.j);
    NoOwnershipTransferUTF8Marshaler.GetInstance(null).CleanUpNativeData(representation.sin);
    Marshal.FreeHGlobal(native_data);
  }

  public override IntPtr MarshalManagedToNativeImplementation(object managed_object) {
    if (!(managed_object is BodyGeopotentialElement value)) {
      throw new NotSupportedException();
    }
    var representation = new Representation{
        degree = NoOwnershipTransferUTF8Marshaler.GetInstance(null).MarshalManagedToNative(value.degree),
        order = NoOwnershipTransferUTF8Marshaler.GetInstance(null).MarshalManagedToNative(value.order),
        cos = NoOwnershipTransferUTF8Marshaler.GetInstance(null).MarshalManagedToNative(value.cos),
        j = NoOwnershipTransferUTF8Marshaler.GetInstance(null).MarshalManagedToNative(value.j),
        sin = NoOwnershipTransferUTF8Marshaler.GetInstance(null).MarshalManagedToNative(value.sin),
    };
    IntPtr buffer = Marshal.AllocHGlobal(Marshal.SizeOf(representation));
    Marshal.StructureToPtr(representation, buffer, fDeleteOld: false);
    return buffer;
  }

  public override object MarshalNativeToManaged(IntPtr native_data) {
    var representation = (Representation)Marshal.PtrToStructure(native_data, typeof(Representation));
    return new BodyGeopotentialElement{
        degree = NoOwnershipTransferUTF8Marshaler.GetInstance(null).MarshalNativeToManaged(representation.degree) as String,
        order = NoOwnershipTransferUTF8Marshaler.GetInstance(null).MarshalNativeToManaged(representation.order) as String,
        cos = NoOwnershipTransferUTF8Marshaler.GetInstance(null).MarshalNativeToManaged(representation.cos) as String,
        j = NoOwnershipTransferUTF8Marshaler.GetInstance(null).MarshalNativeToManaged(representation.j) as String,
        sin = NoOwnershipTransferUTF8Marshaler.GetInstance(null).MarshalNativeToManaged(representation.sin) as String,
    };
  }

  private static readonly BodyGeopotentialElementMarshaler instance_ =
      new BodyGeopotentialElementMarshaler();
}

internal class BodyParametersMarshaler : MonoMarshaler {
  [StructLayout(LayoutKind.Sequential)]
  internal struct Representation {
    public IntPtr name;
    public IntPtr gravitational_parameter;
    public IntPtr reference_instant;
    public IntPtr min_radius;
    public IntPtr mean_radius;
    public IntPtr max_radius;
    public IntPtr axis_right_ascension;
    public IntPtr axis_declination;
    public IntPtr reference_angle;
    public IntPtr angular_frequency;
    public IntPtr reference_radius;
    public IntPtr j2;
    public IntPtr geopotential;
  }

  public static ICustomMarshaler GetInstance(string s) {
    return instance_;
  }

  public override void CleanUpNativeDataImplementation(IntPtr native_data) {
    var representation = (Representation)Marshal.PtrToStructure(native_data, typeof(Representation));
    NoOwnershipTransferUTF8Marshaler.GetInstance(null).CleanUpNativeData(representation.name);
    NoOwnershipTransferUTF8Marshaler.GetInstance(null).CleanUpNativeData(representation.gravitational_parameter);
    NoOwnershipTransferUTF8Marshaler.GetInstance(null).CleanUpNativeData(representation.reference_instant);
    NoOwnershipTransferUTF8Marshaler.GetInstance(null).CleanUpNativeData(representation.min_radius);
    NoOwnershipTransferUTF8Marshaler.GetInstance(null).CleanUpNativeData(representation.mean_radius);
    NoOwnershipTransferUTF8Marshaler.GetInstance(null).CleanUpNativeData(representation.max_radius);
    NoOwnershipTransferUTF8Marshaler.GetInstance(null).CleanUpNativeData(representation.axis_right_ascension);
    NoOwnershipTransferUTF8Marshaler.GetInstance(null).CleanUpNativeData(representation.axis_declination);
    NoOwnershipTransferUTF8Marshaler.GetInstance(null).CleanUpNativeData(representation.reference_angle);
    NoOwnershipTransferUTF8Marshaler.GetInstance(null).CleanUpNativeData(representation.angular_frequency);
    NoOwnershipTransferUTF8Marshaler.GetInstance(null).CleanUpNativeData(representation.reference_radius);
    NoOwnershipTransferUTF8Marshaler.GetInstance(null).CleanUpNativeData(representation.j2);
    RepeatedMarshaler<BodyGeopotentialElement, BodyGeopotentialElementMarshaler>.GetInstance(null).CleanUpNativeData(representation.geopotential);
    Marshal.FreeHGlobal(native_data);
  }

  public override IntPtr MarshalManagedToNativeImplementation(object managed_object) {
    if (!(managed_object is BodyParameters value)) {
      throw new NotSupportedException();
    }
    var representation = new Representation{
        name = NoOwnershipTransferUTF8Marshaler.GetInstance(null).MarshalManagedToNative(value.name),
        gravitational_parameter = NoOwnershipTransferUTF8Marshaler.GetInstance(null).MarshalManagedToNative(value.gravitational_parameter),
        reference_instant = NoOwnershipTransferUTF8Marshaler.GetInstance(null).MarshalManagedToNative(value.reference_instant),
        min_radius = NoOwnershipTransferUTF8Marshaler.GetInstance(null).MarshalManagedToNative(value.min_radius),
        mean_radius = NoOwnershipTransferUTF8Marshaler.GetInstance(null).MarshalManagedToNative(value.mean_radius),
        max_radius = NoOwnershipTransferUTF8Marshaler.GetInstance(null).MarshalManagedToNative(value.max_radius),
        axis_right_ascension = NoOwnershipTransferUTF8Marshaler.GetInstance(null).MarshalManagedToNative(value.axis_right_ascension),
        axis_declination = NoOwnershipTransferUTF8Marshaler.GetInstance(null).MarshalManagedToNative(value.axis_declination),
        reference_angle = NoOwnershipTransferUTF8Marshaler.GetInstance(null).MarshalManagedToNative(value.reference_angle),
        angular_frequency = NoOwnershipTransferUTF8Marshaler.GetInstance(null).MarshalManagedToNative(value.angular_frequency),
        reference_radius = NoOwnershipTransferUTF8Marshaler.GetInstance(null).MarshalManagedToNative(value.reference_radius),
        j2 = NoOwnershipTransferUTF8Marshaler.GetInstance(null).MarshalManagedToNative(value.j2),
        geopotential = RepeatedMarshaler<BodyGeopotentialElement, BodyGeopotentialElementMarshaler>.GetInstance(null).MarshalManagedToNative(value.geopotential),
    };
    IntPtr buffer = Marshal.AllocHGlobal(Marshal.SizeOf(representation));
    Marshal.StructureToPtr(representation, buffer, fDeleteOld: false);
    return buffer;
  }

  public override object MarshalNativeToManaged(IntPtr native_data) {
    var representation = (Representation)Marshal.PtrToStructure(native_data, typeof(Representation));
    return new BodyParameters{
        name = NoOwnershipTransferUTF8Marshaler.GetInstance(null).MarshalNativeToManaged(representation.name) as String,
        gravitational_parameter = NoOwnershipTransferUTF8Marshaler.GetInstance(null).MarshalNativeToManaged(representation.gravitational_parameter) as String,
        reference_instant = NoOwnershipTransferUTF8Marshaler.GetInstance(null).MarshalNativeToManaged(representation.reference_instant) as String,
        min_radius = NoOwnershipTransferUTF8Marshaler.GetInstance(null).MarshalNativeToManaged(representation.min_radius) as String,
        mean_radius = NoOwnershipTransferUTF8Marshaler.GetInstance(null).MarshalNativeToManaged(representation.mean_radius) as String,
        max_radius = NoOwnershipTransferUTF8Marshaler.GetInstance(null).MarshalNativeToManaged(representation.max_radius) as String,
        axis_right_ascension = NoOwnershipTransferUTF8Marshaler.GetInstance(null).MarshalNativeToManaged(representation.axis_right_ascension) as String,
        axis_declination = NoOwnershipTransferUTF8Marshaler.GetInstance(null).MarshalNativeToManaged(representation.axis_declination) as String,
        reference_angle = NoOwnershipTransferUTF8Marshaler.GetInstance(null).MarshalNativeToManaged(representation.reference_angle) as String,
        angular_frequency = NoOwnershipTransferUTF8Marshaler.GetInstance(null).MarshalNativeToManaged(representation.angular_frequency) as String,
        reference_radius = NoOwnershipTransferUTF8Marshaler.GetInstance(null).MarshalNativeToManaged(representation.reference_radius) as String,
        j2 = NoOwnershipTransferUTF8Marshaler.GetInstance(null).MarshalNativeToManaged(representation.j2) as String,
        geopotential = RepeatedMarshaler<BodyGeopotentialElement, BodyGeopotentialElementMarshaler>.GetInstance(null).MarshalNativeToManaged(representation.geopotential) as BodyGeopotentialElement[],
    };
  }

  private static readonly BodyParametersMarshaler instance_ =
      new BodyParametersMarshaler();
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
    var representation = (Representation)Marshal.PtrToStructure(native_data, typeof(Representation));
    NoOwnershipTransferUTF8Marshaler.GetInstance(null).CleanUpNativeData(representation.fitting_tolerance);
    NoOwnershipTransferUTF8Marshaler.GetInstance(null).CleanUpNativeData(representation.geopotential_tolerance);
    Marshal.FreeHGlobal(native_data);
  }

  public override IntPtr MarshalManagedToNativeImplementation(object managed_object) {
    if (!(managed_object is ConfigurationAccuracyParameters value)) {
      throw new NotSupportedException();
    }
    var representation = new Representation{
        fitting_tolerance = NoOwnershipTransferUTF8Marshaler.GetInstance(null).MarshalManagedToNative(value.fitting_tolerance),
        geopotential_tolerance = NoOwnershipTransferUTF8Marshaler.GetInstance(null).MarshalManagedToNative(value.geopotential_tolerance),
    };
    IntPtr buffer = Marshal.AllocHGlobal(Marshal.SizeOf(representation));
    Marshal.StructureToPtr(representation, buffer, fDeleteOld: false);
    return buffer;
  }

  public override object MarshalNativeToManaged(IntPtr native_data) {
    var representation = (Representation)Marshal.PtrToStructure(native_data, typeof(Representation));
    return new ConfigurationAccuracyParameters{
        fitting_tolerance = NoOwnershipTransferUTF8Marshaler.GetInstance(null).MarshalNativeToManaged(representation.fitting_tolerance) as String,
        geopotential_tolerance = NoOwnershipTransferUTF8Marshaler.GetInstance(null).MarshalNativeToManaged(representation.geopotential_tolerance) as String,
    };
  }

  private static readonly ConfigurationAccuracyParametersMarshaler instance_ =
      new ConfigurationAccuracyParametersMarshaler();
}

internal class ConfigurationFixedStepParametersMarshaler : MonoMarshaler {
  [StructLayout(LayoutKind.Sequential)]
  internal struct Representation {
    public IntPtr fixed_step_size_integrator;
    public IntPtr integration_step_size;
  }

  public static ICustomMarshaler GetInstance(string s) {
    return instance_;
  }

  public override void CleanUpNativeDataImplementation(IntPtr native_data) {
    var representation = (Representation)Marshal.PtrToStructure(native_data, typeof(Representation));
    NoOwnershipTransferUTF8Marshaler.GetInstance(null).CleanUpNativeData(representation.fixed_step_size_integrator);
    NoOwnershipTransferUTF8Marshaler.GetInstance(null).CleanUpNativeData(representation.integration_step_size);
    Marshal.FreeHGlobal(native_data);
  }

  public override IntPtr MarshalManagedToNativeImplementation(object managed_object) {
    if (!(managed_object is ConfigurationFixedStepParameters value)) {
      throw new NotSupportedException();
    }
    var representation = new Representation{
        fixed_step_size_integrator = NoOwnershipTransferUTF8Marshaler.GetInstance(null).MarshalManagedToNative(value.fixed_step_size_integrator),
        integration_step_size = NoOwnershipTransferUTF8Marshaler.GetInstance(null).MarshalManagedToNative(value.integration_step_size),
    };
    IntPtr buffer = Marshal.AllocHGlobal(Marshal.SizeOf(representation));
    Marshal.StructureToPtr(representation, buffer, fDeleteOld: false);
    return buffer;
  }

  public override object MarshalNativeToManaged(IntPtr native_data) {
    var representation = (Representation)Marshal.PtrToStructure(native_data, typeof(Representation));
    return new ConfigurationFixedStepParameters{
        fixed_step_size_integrator = NoOwnershipTransferUTF8Marshaler.GetInstance(null).MarshalNativeToManaged(representation.fixed_step_size_integrator) as String,
        integration_step_size = NoOwnershipTransferUTF8Marshaler.GetInstance(null).MarshalNativeToManaged(representation.integration_step_size) as String,
    };
  }

  private static readonly ConfigurationFixedStepParametersMarshaler instance_ =
      new ConfigurationFixedStepParametersMarshaler();
}

internal class ConfigurationAdaptiveStepParametersMarshaler : MonoMarshaler {
  [StructLayout(LayoutKind.Sequential)]
  internal struct Representation {
    public IntPtr adaptive_step_size_integrator;
    public IntPtr length_integration_tolerance;
    public IntPtr speed_integration_tolerance;
  }

  public static ICustomMarshaler GetInstance(string s) {
    return instance_;
  }

  public override void CleanUpNativeDataImplementation(IntPtr native_data) {
    var representation = (Representation)Marshal.PtrToStructure(native_data, typeof(Representation));
    NoOwnershipTransferUTF8Marshaler.GetInstance(null).CleanUpNativeData(representation.adaptive_step_size_integrator);
    NoOwnershipTransferUTF8Marshaler.GetInstance(null).CleanUpNativeData(representation.length_integration_tolerance);
    NoOwnershipTransferUTF8Marshaler.GetInstance(null).CleanUpNativeData(representation.speed_integration_tolerance);
    Marshal.FreeHGlobal(native_data);
  }

  public override IntPtr MarshalManagedToNativeImplementation(object managed_object) {
    if (!(managed_object is ConfigurationAdaptiveStepParameters value)) {
      throw new NotSupportedException();
    }
    var representation = new Representation{
        adaptive_step_size_integrator = NoOwnershipTransferUTF8Marshaler.GetInstance(null).MarshalManagedToNative(value.adaptive_step_size_integrator),
        length_integration_tolerance = NoOwnershipTransferUTF8Marshaler.GetInstance(null).MarshalManagedToNative(value.length_integration_tolerance),
        speed_integration_tolerance = NoOwnershipTransferUTF8Marshaler.GetInstance(null).MarshalManagedToNative(value.speed_integration_tolerance),
    };
    IntPtr buffer = Marshal.AllocHGlobal(Marshal.SizeOf(representation));
    Marshal.StructureToPtr(representation, buffer, fDeleteOld: false);
    return buffer;
  }

  public override object MarshalNativeToManaged(IntPtr native_data) {
    var representation = (Representation)Marshal.PtrToStructure(native_data, typeof(Representation));
    return new ConfigurationAdaptiveStepParameters{
        adaptive_step_size_integrator = NoOwnershipTransferUTF8Marshaler.GetInstance(null).MarshalNativeToManaged(representation.adaptive_step_size_integrator) as String,
        length_integration_tolerance = NoOwnershipTransferUTF8Marshaler.GetInstance(null).MarshalNativeToManaged(representation.length_integration_tolerance) as String,
        speed_integration_tolerance = NoOwnershipTransferUTF8Marshaler.GetInstance(null).MarshalNativeToManaged(representation.speed_integration_tolerance) as String,
    };
  }

  private static readonly ConfigurationAdaptiveStepParametersMarshaler instance_ =
      new ConfigurationAdaptiveStepParametersMarshaler();
}

internal class OrbitAnalysisMarshaler : MonoMarshaler {
  [StructLayout(LayoutKind.Sequential)]
  internal struct Representation {
    public double progress_of_next_analysis;
    public int primary_index;
    public double mission_duration;
    public IntPtr elements;
    public IntPtr recurrence;
    public IntPtr ground_track;
  }

  public static ICustomMarshaler GetInstance(string s) {
    return instance_;
  }

  public override void CleanUpNativeDataImplementation(IntPtr native_data) {
    var representation = (Representation)Marshal.PtrToStructure(native_data, typeof(Representation));
    OwnershipTransferMarshaler<OrbitalElements, OptionalMarshaler<OrbitalElements>>.GetInstance(null).CleanUpNativeData(representation.elements);
    OwnershipTransferMarshaler<OrbitRecurrence, OptionalMarshaler<OrbitRecurrence>>.GetInstance(null).CleanUpNativeData(representation.recurrence);
    OwnershipTransferMarshaler<OrbitGroundTrack, OptionalMarshaler<OrbitGroundTrack>>.GetInstance(null).CleanUpNativeData(representation.ground_track);
    Marshal.FreeHGlobal(native_data);
  }

  public override IntPtr MarshalManagedToNativeImplementation(object managed_object) {
    if (!(managed_object is OrbitAnalysis value)) {
      throw new NotSupportedException();
    }
    var representation = new Representation{
        progress_of_next_analysis = value.progress_of_next_analysis,
        primary_index = value.primary_index,
        mission_duration = value.mission_duration,
        elements = OwnershipTransferMarshaler<OrbitalElements, OptionalMarshaler<OrbitalElements>>.GetInstance(null).MarshalManagedToNative(value.elements),
        recurrence = OwnershipTransferMarshaler<OrbitRecurrence, OptionalMarshaler<OrbitRecurrence>>.GetInstance(null).MarshalManagedToNative(value.recurrence),
        ground_track = OwnershipTransferMarshaler<OrbitGroundTrack, OptionalMarshaler<OrbitGroundTrack>>.GetInstance(null).MarshalManagedToNative(value.ground_track),
    };
    IntPtr buffer = Marshal.AllocHGlobal(Marshal.SizeOf(representation));
    Marshal.StructureToPtr(representation, buffer, fDeleteOld: false);
    return buffer;
  }

  public override object MarshalNativeToManaged(IntPtr native_data) {
    var representation = (Representation)Marshal.PtrToStructure(native_data, typeof(Representation));
    return new OrbitAnalysis{
        progress_of_next_analysis = representation.progress_of_next_analysis,
        primary_index = representation.primary_index,
        mission_duration = representation.mission_duration,
        elements = OwnershipTransferMarshaler<OrbitalElements, OptionalMarshaler<OrbitalElements>>.GetInstance(null).MarshalNativeToManaged(representation.elements) as OrbitalElements?,
        recurrence = OwnershipTransferMarshaler<OrbitRecurrence, OptionalMarshaler<OrbitRecurrence>>.GetInstance(null).MarshalNativeToManaged(representation.recurrence) as OrbitRecurrence?,
        ground_track = OwnershipTransferMarshaler<OrbitGroundTrack, OptionalMarshaler<OrbitGroundTrack>>.GetInstance(null).MarshalNativeToManaged(representation.ground_track) as OrbitGroundTrack?,
    };
  }

  private static readonly OrbitAnalysisMarshaler instance_ =
      new OrbitAnalysisMarshaler();
}

}  // namespace ksp_plugin_adapter
}  // namespace principia
