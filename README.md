# DiscountCode_App

1. When you start the gRPC service -- copy the URL (Host URL) i.e. -> "https://localhost:7046"
2. Open postman (it will work as client)
3. You can click on "NEW" and choose "gRPC" from list
4. Upload .proto file in the file selection (Import proto file section will be there)
5. Paste your server URL in url
6. Choose method accordingly which you need to execute (Generate or UseCode)
7. Request params for Generate method -
{
  "count": 10,
  "length": 7
}
8. Request params for UseCode method -
{
    "code":"Y5QNADZ"
}
   
