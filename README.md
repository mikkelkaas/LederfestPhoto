# LederfestPhoto

## Backend
You need a azure blob storage and a SQL database.

Store the connection strings in appsettings :

```json

  "ConnectionStrings": {
    "lederfestPhotoContext": "**",
    "AzureCloudStorage": "**"
  }
}
```

## Website
website is in "website" folder.

develop using "ng serve"
make production build using "ng build --prod --base-href "/lederfest/" (if served in a subfolder called 'lederfest', otherwise ommit the --base-href part)