# openstig-api-upload
This is the openSTIG Upload API for uploading a CKL file. It has two calls.

POST to / to save a new checklist

PUT to /{id} to update a new checklist content but keep the rest in tact

/swagger/ gives you the API structure.

## creating the user
* ~/mongodb/bin/mongo 'mongodb://root:myp2ssw0rd@localhost'
* use admin
* db.createUser({ user: "openstig" , pwd: "openstig1234!", roles: ["readWriteAnyDatabase"]});
* use openstig
* db.createCollection("Templates");

## connecting to the database collection straight
~/mongodb/bin/mongo 'mongodb://openstig:openstig1234!@localhost/openstig?authSource=admin'