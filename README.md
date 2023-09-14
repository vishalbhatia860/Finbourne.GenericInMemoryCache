# Generic In-Memory Cache with LRU Eviction Policy in C# .NET core

This README provides an overview and usage guide for a generic in-memory cache project with an LRU (Least Recently Used) eviction policy implemented in C# .NET core. This cache is designed to help you efficiently store and manage key-value pairs in memory.

## Table of Contents

1. [Introduction](#introduction)
2. [Features](#features)
3. [Installation](#installation)
4. [Usage](#usage)
   - [Initialization](#initialization)
   - [Adding Key-Value Pairs](#adding-key-value-pairs)
   - [Accessing Key-Value Pairs](#accessing-key-value-pairs)
   - [Eviction Policy](#eviction-policy)


## Introduction

This in-memory cache project provides a simple and generic way to store key-value pairs in memory using C# .NET. The cache uses the LRU eviction policy to automatically remove the least recently used items when the cache reaches its maximum capacity.

## Features

- Generic key-value storage.
- LRU eviction policy.
- Customizable maximum cache size.
- Thread-safe operations.
- Easy-to-use API.

## Installation

To use this in-memory cache project in your C# .NET core application, you can either:
   
- Download the source code and add it to your project.
- Install it via NuGet Package Manager using the following command:

   ```bash
   Install-Package Finbourne.GenericInMemoryCache
   ```

## Usage

### Initialization

To use the cache, you first need to initialize it with your desired maximum capacity:

 ```bash
 CacheConfiguration config = new CacheConfiguration ( { MaxCacheSize = 3 } );

 // logger instance of type Microsoft.Extensions.Logging;

 IGenericInMemoryCache cache = new InMemoryCache(config, logger);
 ```


### Adding Key-Value Pairs


```bash
 cache.SetCacheAsync<string>("color", "red").Wait();

 cache.SetCacheAsync<int?>("number", 1).Wait();
```


### Accessing Key-Value Pairs

```bash
  value = cache.GetCacheAsync<string>("key1").Result

  if (value is null) {
	Console.WriteLine("key1 not found in the cache.");
  }
  else {
    Console.WriteLine($"key1 found in the cache with value {value}");
  }
```

### Eviction Policy

When the cache reaches its maximum capacity, it will automatically remove the least recently used items to make space for new entries. 
You don't need to manage this process manually.
