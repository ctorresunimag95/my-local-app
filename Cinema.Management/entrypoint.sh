#!/bin/sh

cosmosHost=cosmos-db
cosmosPort=8081

# wait for cosmosdb to be available, a health check is being called
echo "Waiting for cosmosdb emulator to be available at $cosmosHost:$cosmosPort ..."
until [ "$(curl -k -s --connect-timeout 5 -o /dev/null -w '%{http_code}' https://$cosmosHost:$cosmosPort/_explorer/emulator.pem)" == "200" ]; do
  sleep 3
done
echo "CosmosDB is available"

# Download the cosmosDB cert and add it to the trusted certs
echo "Downloading cosmosdb cert"
curl -k https://$cosmosHost:$cosmosPort/_explorer/emulator.pem > emulator.crt

echo "Adding cosmosdb cert to trusted certs"
cp emulator.crt /usr/local/share/ca-certificates/
update-ca-certificates

echo "Running management service!"
dotnet Cinema.Management.dll