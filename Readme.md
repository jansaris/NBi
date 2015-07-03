![Logo](https://github.com/Seddryck/nbi/raw/master/NBi-logo-white.jpg)
# NBi #
NBi is a **testing framework** (add-on to NUnit) for Microsoft **Business Intelligence** platform (SQL Server Database engine, SSIS, SSAS, SSRS) and Data Access.

The main goal of this framework is to let users create tests with a declarative approach based on an **Xml** syntax. By the means of NBi, you don't need to develop C# code to specify your tests! Either, you don't need Visual Studio to compile your test suite. Just create an Xml file and let the framework interpret it and play your tests. The framework is designed as an add-on of NUnit but with the possibility to port it easily to other testing frameworks.

![project status](http://stillmaintained.com/Seddryck/nbi.png)

## Releases ##
Binaries for the different releases are hosted on [www.nbi.io](http://www.nbi.io/release/) or [GitHub] (https://github.com/Seddryck/NBi/releases)

## Documentation ##
The documentation is available on-line and is hosted on [www.nbi.io](http://www.nbi.io/docs/home/)

## Licenses ##
NBi is available with two licenses: MS-PL and Apache 2.0. You're free to choose which one convains the best to your project. NBi is also using several OSS projects as librairies. All these projects and their licenses are available in the folder "License". 

## Bugs, issues and requests for features ##
The list of bugs and feature's requests is hosted on [GitHub](https://github.com/Seddryck/NBi/issues)

## Continuous Integration ##
A continuous integration service is available on AppVeyor at [https://ci.appveyor.com/project/CdricLCharlier/nbi/]
Note that all the tests are not executed on this environment due to limitations in the availability of some components.

- Unit tests are always executed
- Integration tests are executed if the corresponding component is available
    - Database Engine: Yes. Due to the usage of an Azure database to run these tests, these tests are enabled on the CI platform
    - OLAP Engine: No
    - ETL Engine (SSIS): No
    - Windows Service: No (but planned to integrate them)
    - Local Database: No (but planned to integrate them)
    - Report Server: No (but planned to integrate them)
- Acceptance tests are excluded

[![Build status](https://ci.appveyor.com/api/projects/status/t5m0hr57vnsdv0v7)](https://ci.appveyor.com/project/CdricLCharlier/nbi)

Two artefacts are provided by this CI:

- Framework.zip contains the dll needed to run tests written with NBi
- UI.zip contains the exe and dlls needed to run Genbi

## Code ##
NBi is using **Git** as DCVS and the code is hosted on [Github](https://github.com/Seddryck/NBi). A mirror copy is hosted on [Bitbucket](http://bitbucket.org/Seddryck/nbi).

### Automated Testing ###
NBi has around 1300 automated tests, asserting a lot of features before each release. These tests are organized in three folders:

- Acceptance: The tests are effectively written in nbits file and played end-to-end by the framework itself. They don't use any fake, mock or stub and are effectively connected to real database and cubes and perform query on them.
- Integration: These tests are used to assert interactions with resources such as databases or cubes. The make usage of stubs to define parameters impacting the code to use.
- Unit: These tests are never contacting an external resource and tests maximum the code of one class. Usage of stubs, fakes and mocks is welcome.

In order to be able to build the software on different machines, the database and cube used during tests must always be Adventure Works 2008R2. In order to facilitate the integration, NBi is connected by default to the online SQL database hosted on Azure (Unfortunatelly no equivalent for SSAS). If you want to overrides the connection settings for executing the tests on your own environement just create a file named ConnectionString.user.config in the folder "NBi.Testing" and copy the content from the file ConnectionString.config into it before adjusting for your environement.

## Tracking ##
This OSS project is tracked by [Ohloh](http://www.ohloh.net/p/NBi)

[![Project Stats](https://www.ohloh.net/p/nbi/widgets/project_thin_badge.gif)](https://www.ohloh.net/p/YOUR_PROJECT)
