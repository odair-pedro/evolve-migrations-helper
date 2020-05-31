# Evolve Migrations Helper
[![Build status](https://ci.appveyor.com/api/projects/status/199ocf60nyj20fa8/branch/master?svg=true)](https://ci.appveyor.com/project/odair-pedro/evolve-migrations-helper/branch/master)
[![Codacy Badge](https://api.codacy.com/project/badge/Grade/d26a83a7802345b6808e7719094aa01c)](https://app.codacy.com/manual/odair-pedro/evolve-migrations-helper?utm_source=github.com&utm_medium=referral&utm_content=odair-pedro/evolve-migrations-helper&utm_campaign=Badge_Grade_Dashboard)

A simple tool for help you to versionate [Evolve](https://github.com/lecaillon/Evolve) migrations.

If you think that Evolve is an amazing tool, you will like to use this helper. :blush:

## Usage
Run the follow command to create a _dataset_ migration:
```
migrations add-dataset MyDatasetMigration
```
Run the follown command to create a _scheme_ migration:
```
migrations add-migration MySchemeMigration
```

Or run the help command to see all the options:
```
migrations --help
```
```
Usage: migrations [command] [options]

Commands:
    add-dataset          Add a new migration file (on path: "./datasets")
    add-migration        Add a new migration file (on path: "./migrations")

Options:
    -s|--separator       The file name seperator. Default is '__' (Double underscore). Eg: 'v20200530193319__MyMigration.sql'

```

### Note
The created file will be configured as embedded resource in your csproj file. 

If you don't like that behavior, are welcome to contribute with this project! :heart:

## Download
The latest version can be found [here](https://github.com/odair-pedro/evolve-migrations-helper/releases).
