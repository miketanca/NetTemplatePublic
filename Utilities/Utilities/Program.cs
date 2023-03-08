// See https://aka.ms/new-console-template for more information
using Utilities;

var classificationSql = await SchemaUtils.AddClassifications(
    "Server=localhost;Database=web-ua;Integrated Security=True;TrustServerCertificate=True"
);

Console.WriteLine("Done!");
