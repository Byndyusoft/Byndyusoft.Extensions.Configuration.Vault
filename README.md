# Byndyusoft.Extensions.Configuration.Vault

[![(License)](https://img.shields.io/github/license/Byndyusoft/Byndyusoft.Extensions.Configuration.Vault.svg)](LICENSE.txt)
[![Nuget](http://img.shields.io/nuget/v/Byndyusoft.Extensions.Configuration.Vault.svg?maxAge=10800)](https://www.nuget.org/packages/Byndyusoft.Extensions.Configuration.Vault/) [![NuGet downloads](https://img.shields.io/nuget/dt/Byndyusoft.Extensions.Configuration.Vault.svg)](https://www.nuget.org/packages/Byndyusoft.Extensions.Configuration.Vault/) 

The package is based on [VaultSharp](https://github.com/rajanadar/VaultSharp) library and provides an extension method called `AddVault` for `IConfigurationBuilder`. 
Package adds to builder `VaultConfigurationProvider` which works with secrets stored in Vault's [KV Secrects Engine](https://www.vaultproject.io/docs/secrets/kv). 

## Installing

```shell
dotnet add package Byndyusoft.Extensions.Configuration.Vault
```

## Usage

A minimal example is shown below:
```
var builder = new ConfigurationBuilder()
    .AddVault("http://my.vault.com", new TokenAuthMethodInfo("token"), engineName: "kv");
```
Here are:

* **`http://my.vault.com`** 

	Uri of the Vault Server.
	
* **`new TokenAuthMethodInfo("token")`** 

	Authentication method. Supports all [authentication methods](https://github.com/rajanadar/VaultSharp#auth-methods) supported by the Vault Service.
	
* **`kv`** 

	The name of the Vault KV Secrects Engine.

## Options

`AddVault` has an overloads with an additional parameter of type `Action<VaultConfigurationSource>` which allows the options outlined below to be set.

* **`Engine.Version`**
  
   The version of the engine. Default is V2. You can read more about engine versioning in [official docs](https://www.vaultproject.io/docs/secrets/kv).


* **`Engine.Namespace`**
  
   The name of namespace the engine belongs to. Default is null. You can read more about Vault Namespaces in [official docs](https://www.vaultproject.io/docs/enterprise/namespaces).

* **`Timeout`**
  
   The amount of time the client should wait before timing out when calling to Vault. Default is null (infinite).

* **`Optional`**

   A `bool` that indicates whether the engine is optional. If `false` then it will throw during the first load if the config is missing for the given engine. Defaults to `false`.

* **`ReloadOnChange`**

   A `bool` indicating whether to reload the engine when it changes in Vault.
   If `true` it will watch the configured engine for changes. When a change occurs the config will be asynchronously reloaded and the `IChangeToken` will be triggered to signal that the config has been reloaded. Defaults to `false`.

* **`ReloadDelay`**
   
   The amount of time that reload will wait before calling Load. Defaults to 1 minute.

```
var builder = new ConfigurationBuilder()
    .AddVault("http://my.vault.com", new TokenAuthMethodInfo("token"), engineName: "kv",
		vault => 
		{
			vault.Engine.Version = VaultKeyValueEngineVersion.V2;
			vault.Engine.Namespace = "enterprise vault only namespace";
			vault.Timeout = TimeSpan.FromSeconds(5);
			vault.Optional = false;
			vault.ReloadOnChange = true;
			vault.ReloadDelay = TimeSpan.FromMinutes(1);
		});
```

# Contributing

To contribute, you will need to setup your local environment, see [prerequisites](#prerequisites). For the contribution and workflow guide, see [package development lifecycle](#package-development-lifecycle).

A detailed overview on how to contribute can be found in the [contributing guide](CONTRIBUTING.md).

## Prerequisites

Make sure you have installed all of the following prerequisites on your development machine:

- Git - [Download & Install Git](https://git-scm.com/downloads). OSX and Linux machines typically have this already installed.
- .NET Core (version 3.0 or higher) - [Download & Install .NET Core](https://dotnet.microsoft.com/download/dotnet-core/3.1).

## General folders layout

### src
- source code

### tests

- unit-tests

## Package development lifecycle

- Implement package logic in `src`
- Add or addapt unit-tests (prefer before and simultaneously with coding) in `tests`
- Add or change the documentation as needed
- Open pull request in the correct branch. Target the project's `master` branch

# Maintainers

[github.maintain@byndyusoft.com](mailto:github.maintain@byndyusoft.com)