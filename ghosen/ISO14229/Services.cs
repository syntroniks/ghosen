using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ghosen.ISO14229
{
	public enum Services
	{
		// Diagnostic and communication management functional unit
		DiagnosticSessionControl = 0x10, // The client requests to control a diagnostic session with a server(s). 
		ECUReset = 0x11, // The client forces the server(s) to perform a reset.
		SecurityAccess = 0x27, // The client requests to unlock a secured server(s).
		CommunicationControl = 0x28, // The client requests the server to control its communication.
		TesterPresent = 0x3E,  // The client indicates to the server(s) that it is still present.
		AccessTimingParameter = 0x83, // The client uses this service to read/modify the timing parameters for an active communication.
		SecuredDataTransmission = 0x84, // The client uses this service to perform data transmission with an extended data link security.
		ControlDTCSetting = 0x85, // The client controls the setting of DTCs in the server.
		ResponseOnEvent = 0x86, // The client requests to start an event mechanism in the server.
		LinkControl = 0x87, // The client requests control of the communication baud rate. 

		// Data transmission functional unit
		ReadDataByIdentifier = 0x22, // The client requests to read the current value of a record identified by a provided data Identifier.
		ReadMemoryByAddress = 0x23,  // The client requests to read the current value of the provided memory range.
		ReadScalingDataByIdentifier = 0x24, // The client requests to read the scaling information of a record identified by a provided dataIdentifier.
		ReadDataByPeriodicIdentifier = 0x2A, // The client requests to schedule data in the server for periodic transmission.
		DynamicallyDefineDataIdentifier = 0x2C, // The client requests to dynamically define data Identifiers that may subsequently be read by the readDataByIdentifier service.
		WriteDataByIdentifier = 0x2E, // The client requests to write a record specified by a provided dataIdentifier.
		WriteMemoryByAddress = 0x3D, // The client requests to overwrite a provided memory range. 

		// documentation currently missing.
		// So far it is wikipedia only
		RequestDownload = 0x34, // Downloading new software or other data into the control unit is introduced using the "Request Download". Here, the location and size of the data is specified.In turn, the controller specifies how large the data packets can be.
		RequestUpload = 0x35, // The service "request upload" is almost identical to the service "Request Download". With this service, the software from the control unit is transferred to the tester. The location and size must be specified.Again, the size of the data blocks are specified by the tester.
		TransferData = 0x36, // For the actual transmission of data, the service "Transfer Data" is used.This service is used for both uploading and downloading data.The transfer direction is notified in advance by the service "Request Download" or "Upload Request". This service should try to send packets at maximum length, as specified in previous services. If the data set is larger than the maximum, the "Transfer Data" service must be used several times in succession until all data has arrived.
		RequestTransferExit = 0x37, // A data transmission can be 'completed' when using the "Transfer Exit" service.This service is used for comparison between the control unit and the tester.When it is running, a control unit can answer negatively on this request to stop a data transfer request.This will be used when the amount of data (set in "Request Download" or "Upload Request") has not been transferred.
		RequestFileTransfer = 0x38 // This service is used to initiate a file download from the client to the server or upload from the server to the client. Additionally information about the file system are available by this service.
	}
}
