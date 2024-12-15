## Introduction
The openHAB Windows application is a native client for openHAB 3 & 4, leveraging the REST API to render sitemaps and provide a seamless interface for managing your smart home devices.

openHAB (open Home Automation Bus) is an open-source, technology-agnostic home automation platform that integrates various smart home systems and technologies into one solution. It is designed to be flexible and scalable, allowing users to automate their homes with ease. openHAB supports a wide range of devices and protocols, making it a versatile choice for home automation enthusiasts.

### Key Features
- **Extensibility**: With a modular architecture, openHAB can be extended with add-ons to support new devices and technologies.
- **Community-Driven**: Maintained by a vibrant community of developers and users who contribute to its continuous improvement.
- **Cross-Platform**: Runs on various operating systems, including Windows, macOS, Linux, and Raspberry Pi.
- **User-Friendly Interfaces**: Offers multiple user interfaces, such as web-based UIs, mobile apps, and voice control, to manage and monitor your smart home.

For more information, visit the [official openHAB website](https://www.openhab.org/).

## Builds
[![openHAB Build and Release app](https://github.com/openhab/openhab-windows/actions/workflows/openhab.yml/badge.svg)](https://github.com/openhab/openhab-windows/actions/workflows/openhab.yml)

## Code Analysis
The project utilizes SonarQube, hosted by SonarCloud, to analyze the code for potential issues and ensure high code quality.

[![SonarCloud](https://sonarcloud.io/images/project_badges/sonarcloud-white.svg)](https://sonarcloud.io/dashboard?id=openhab_openhab-windows)

### Quality Status

| Branch | Quality Gate Status | Bugs | Code Smells |
|--------|---------------------|------|-------------|
| beta   |                     |      |             |
| main   | [![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=openhab_openhab-windows&metric=alert_status)](https://sonarcloud.io/dashboard?id=openhab_openhab-windows) | [![Bugs](https://sonarcloud.io/api/project_badges/measure?project=openhab_openhab-windows&metric=bugs)](https://sonarcloud.io/dashboard?id=openhab_openhab-windows) | [![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=openhab_openhab-windows&metric=code_smells)](https://sonarcloud.io/dashboard?id=openhab_openhab-windows) |

## Setting up the Development Environment

If you want to contribute to the Windows application, we are here to help you set up your development environment. The openHAB Windows app is developed using Visual Studio 2019 and later versions.

### Steps to Set Up:
1. Download and install [Visual Studio Community Edition](https://www.visualstudio.com/downloads/).
2. During installation, ensure to select UWP SDK 17763.
3. Check out the latest code from GitHub.
4. Open the project in Visual Studio (File -> Open -> Project/Solution).
5. Rebuild the solution to fetch all missing NuGet packages.

You are now ready to contribute!

Before writing any code, please review our [contribution guidelines](https://github.com/openhab/openhab.windows/blob/master/CONTRIBUTING.md).

## Trademark Disclaimer

Product names, logos, brands, and other trademarks referred to within the openHAB website are the property of their respective trademark holders. These trademark holders are not affiliated with openHAB or our website. They do not sponsor or endorse our materials.
