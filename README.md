# TMO Gateway Monitor
A WPF-based C# application to monitor a TMobile Gateway. This utility used the API that is hosted on the Arcadyan gateway which is the same as is used by the T-Mobile phone application. 

![image](https://user-images.githubusercontent.com/45472311/193646433-5cf73ed8-e6e7-490c-ae75-232b58c3a22b.png)

### Features
* 4G and 5G signal (band, bars, tower connection, signal strength, ...)
* Gateway device properties
* SIM information (IMEI, ICCID, IMSI, MSISDN [TMO phone number]
* Raw JSON data from API
* Logging to CSV file
* Reboot gateway

I created this app for the purpose of monitoring/debugging signal issues I was having. When running, the application will poll the service every couple of seconds. In order to retrieve telemetry data (device list, cell information, ...), you must provide your password for the gateway admin.  The URL defaults to the standard IP address (192.168.12.1). Currently the Cell Information tab does not show any data, this will be worked on next.

### Logging
If logging is enabled, a file called "GatewayLog.csv" will be created in the directory the application is running. Even though the live display is refreshed every few seconds, log entries are only created when the 4G or 5G signal changes. If the file is open while the application is running, the log file can not be accessed by the application. The application will store these results in memory however and will write the entries as soon as the file is available again. 
