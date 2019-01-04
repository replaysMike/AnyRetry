# AnyRetry

[![nuget](https://img.shields.io/nuget/v/AnyRetry.svg)](https://www.nuget.org/packages/AnyRetry/)
[![nuget](https://img.shields.io/nuget/dt/AnyRetry.svg)](https://www.nuget.org/packages/AnyRetry/)
[![Build status](https://ci.appveyor.com/api/projects/status/25qjrjyhxv8t3dm7?svg=true)](https://ci.appveyor.com/project/MichaelBrown/anyretry)
[![Codacy Badge](https://api.codacy.com/project/badge/Grade/8001bb10a20c4456a98ed4dde145350a)](https://app.codacy.com/app/replaysMike/AnyRetry?utm_source=github.com&utm_medium=referral&utm_content=replaysMike/AnyRetry&utm_campaign=Badge_Grade_Dashboard)
[![Codacy Badge](https://api.codacy.com/project/badge/Coverage/85f671af543f46a599cafd10dab36e5a)](https://www.codacy.com/app/replaysMike/AnyRetry?utm_source=github.com&utm_medium=referral&utm_content=replaysMike/AnyRetry&utm_campaign=Badge_Coverage)

A simple CSharp library for retrying operations with backoff and async support.

## Description

AnyRetry allows you to retry operations that may fail such as database operations, network or anything which may throw an unexpected temporary exception. It supports auto-backoff using different available algorithms which is real handy for retrying network operations.

## Installation
Install AnyRetry from the Package Manager Console:
```
PM> Install-Package AnyRetry
```

## Usage

Simplest usage is simply retry calling a method if it fails.
If any exception is thrown, it will retry up to 10 times every 5 seconds. If it fails every time, a RetryTimeoutException will be thrown.

```csharp
using AnyRetry;

var maxRetries = 10;
var retryEvery = TimeSpan.FromSeconds(5);
Retry.Do(() =>
{
    DoMyNetworkOperation();
}, retryEvery, maxRetries);

```

To only retry if a specific known exception is thrown, but you want to abort if any other type of exception is thrown:

```csharp
var maxRetries = 10;
var retryEvery = TimeSpan.FromSeconds(5);
Retry.Do(() =>
{
    DoMyNetworkOperation();
}, retryEvery, maxRetries, typeof(SocketException), typeof(IOException));
```

To use a backoff schedule (to increase your retry timeouts exponentially for example) simply specify the retry policy:
```csharp
var maxRetries = 10;
var retryEvery = TimeSpan.FromSeconds(5);
Retry.Do(() =>
{
    DoMyNetworkOperation();
}, retryEvery, maxRetries, RetryPolicy.ExponentialBackoff);
```

### Retry Policies

There are 3 types of policies available: `StaticDelay`, `ExponentialBackoff`, `EasedBackoff`. 

StaticDelay specifies that the retry time is always the same. ExponentialBackoff will multiply the `retryEvery` exponentially on every failure (a maximum can also be specified). EasedBackoff allows you to choose a standard easing algorithm, such as `ExponentialEaseOut` or `QuadraticEaseOut` for example.

Specifying policy options:
```csharp
var maxRetries = 10;
var retryEvery = TimeSpan.FromSeconds(5);
var policyOptions = new RetryPolicyOptions { 
    EasingFunction = EasingFunction.QuadraticEaseOut,
    MaxRetryInterval = TimeSpan.FromSeconds(30);
};
Retry.Do(() =>
{
    DoMyNetworkOperation();
}, retryEvery, maxRetries, RetryPolicy.EasedBackoff, policyOptions);
```

### Asynchronous usage

Async usage is identical to the synchronous examples:

```csharp
using AnyRetry;

var maxRetries = 10;
var retryEvery = TimeSpan.FromSeconds(5);
await Retry.DoAsync(async () =>
{
    await DoMyNetworkOperationAsync();
}, retryEvery, maxRetries);
```

### Additional tips

If you need access to the retry information, you can specify access as follows:

```csharp
using AnyRetry;

var maxRetries = 10;
var retryEvery = TimeSpan.FromSeconds(5);
 Retry.Do((retryIteration, maxRetryCount) =>
{
    Console.WriteLine($"Retry #: {retryIteration}");
    Console.WriteLine($"MaxRetries #: {maxRetryCount}");
    DoMyNetworkOperation();
}, retryEvery, maxRetries);
```
