# FroniusDataloggingSolution
I use this logger to query my Fronius IG Plus 35 Inverter.
It is running as a systemd service on a raspberry Pi4.
The interface card is connected to a prolific usb to rs232 adapter.

The pi OS is installed on a 128GB ssd, connected with an usb to sata converter.
The data is stored in an influxdb database and the visualization is done with grafana
