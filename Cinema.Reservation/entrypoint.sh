#!/bin/sh

# wait for service-bus emulator to be ready
WAIT_TIME=60
echo "Waiting for $WAIT_TIME seconds for Service Bus Emulator to be ready..."
sleep $WAIT_TIME

echo "Running reservation service!"
dotnet Cinema.Reservation.dll