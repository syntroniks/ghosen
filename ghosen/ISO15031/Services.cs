namespace ghosen.ISO15031
{
  public enum CurrentDataPIDs
  {
    Keepalive = 0x00,

    FuelSystemStatus = 0x03,
    CalculatedLoad = 0x04,
    EngineCoolantTemperature = 0x05,
    ShortTermFuelTrim_1_3 = 0x06,
    LongTermFuelTrim_1_3 = 0x07,
    ShortTermFuelTrim_2_4 = 0x08,
    LongTermFuelTrim_2_4 = 0x09,
    FuelRailPressure = 0x0A,
    IntakeMAP = 0x0B,
    EngineRPM = 0x0C,
    VehicleSpeed = 0x0D,
    TimingAdvance = 0x0E,
    IntakeAirTemperature = 0x0F,
    MassAirFlow = 0x10,
    AbsoluteThrottlePosition = 0x11,
    SecondaryAirStatus = 0x12,
    O2SensorLocation = 0x13,
    O2_1_1 = 0x14,
    O2_1_2,
    O2_1_3,
    O2_1_4,
    O2_2_1,
    O2_2_2,
    O2_2_3,
    O2_2_4,
    OBDRequirements = 0x1C,
    O2SensorLocation_1 = 0x1D,
    PTOStatus = 0x1E,
    TimeSinceEngineStart = 0x1F,
    DistanceTravelledWithMIL = 0x21,
    FuelRailRelativePressure = 0x22,
    FuelRailPressure_1 = 0x23,
    WBO2_1_1 = 0x24,
    WBO2_1_2 = 0x25,
    WBO2_1_3 = 0x26,
    WBO2_1_4 = 0x27,
    WBO2_2_1 = 0x28,
    WBO2_2_2 = 0x29,
    WBO2_2_3 = 0x2A,
    WBO2_2_4 = 0x2B,
    CommandedEGR = 0x2C,
    EGRError = 0x2D,
    EVAPPurge = 0x2E,
    FuelLevel = 0x2F,

    AbsoluteLoad = 0x43,
    CommandedEquivalenceRatio = 0x44,
    RelativeThrottlePosition = 0x45,
    AmbientAirTemperature = 0x46,

  }

  public enum Infotypes
  {
    MessageCountVIN = 0x01,
    VIN = 0x02,
    MessageCountCALID = 0x03,
    CALID = 0x04,
    MessageCountCVN = 0x05,
    CVN = 0x05,
    MessageCountECUName = 0x09,
    ECUName = 0x0A,

  }

  public enum ServiceType
  {
    CurrentData = 0x01,
    FreezeData = 0x02,
    DiagnosticTroubleCodes = 0x03,
    ClearDiagnosticInformation = 0x04,
    O2SensorData = 0x05,
    MonitoringTestResults = 0x06,
    RecentDiagnosticTroubleCodes = 0x07,
    RequestControl = 0x08,
    VehicleInformation = 0x09,

    NegativeResponse = 0x7F
  }

  public enum NegativeResponseCodes
  {
    GeneralReject = 0x10,
    ServiceNotSupported = 0x11,
    SubFunctionNotSupported = 0x12,
    Busy = 0x21,
    ConditionsNotCorrect = 0x22,
    ResponsePending = 0x78,
  }

  public enum ServiceMessageType
  {
    Request,
    Response
  }
}
