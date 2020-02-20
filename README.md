# AnyRetry

[![nuget](https://img.shields.io/nuget/v/AnyRetry.svg)](https://www.nuget.org/packages/AnyRetry/)
[![nuget](https://img.shields.io/nuget/dt/AnyRetry.svg)](https://www.nuget.org/packages/AnyRetry/)
[![Build status](https://ci.appveyor.com/api/projects/status/25qjrjyhxv8t3dm7?svg=true)](https://ci.appveyor.com/project/MichaelBrown/anyretry)
[![Codacy Badge](https://api.codacy.com/project/badge/Grade/c933a86542a844889cdd64df99328b09)](https://www.codacy.com/app/replaysMike/AnyRetry?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=replaysMike/AnyRetry&amp;utm_campaign=Badge_Grade)
[![Codacy Badge](https://api.codacy.com/project/badge/Coverage/c933a86542a844889cdd64df99328b09)](https://www.codacy.com/app/replaysMike/AnyRetry?utm_source=github.com&utm_medium=referral&utm_content=replaysMike/AnyRetry&utm_campaign=Badge_Coverage)

A simple CSharp library for retrying operations with backoff and async support.

## Description

AnyRetry allows you to retry operations that may fail such as database operations, network or anything which may throw an unexpected temporary exception. It supports auto-backoff using different available algorithms which is real handy for retrying network operations. Support is available for .Net Framework 4+ and .Net Standard 2.0+

## Installation
Install AnyRetry from the Package Manager Console:
```
PM> Install-Package AnyRetry
```

## Usage

Basic usage would be to retry execution of a method if it throws an exception.
If any exception is thrown, it will retry up to 10 times every 5 seconds (configurable). If it exceeds the maximum number of retries, a `RetryTimeoutException` will be thrown.

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

`StaticDelay` specifies that the retry time is always the same. `ExponentialBackoff` will multiply the `retryEvery` exponentially on every failure (a maximum can also be specified). `EasedBackoff` allows you to choose a standard easing algorithm, such as `ExponentialEaseOut` or `QuadraticEaseOut` for example.

Specifying policy options:
```csharp
var maxRetries = 10;
var retryEvery = TimeSpan.FromSeconds(5);
var policyOptions = new RetryPolicyOptions { 
    EasingFunction = EasingFunction.QuadraticEaseOut,
    MaxRetryInterval = TimeSpan.FromSeconds(30),
    // optional: specify we want to ease over 5 retries, not the default of maxRetries=10
    MaxRetrySteps = 5
};
Retry.Do(() =>
{
    DoMyNetworkOperation();
}, retryEvery, maxRetries, RetryPolicy.EasedBackoff, policyOptions);
```
If you wish to specify easing over a period shorter than the maximum number of retries, use the `MaxRetrySteps` option and specify a value shorter than the maximum number of retries. When it hits this limit it will plateau and use the `MaxRetryInterval` for the remaining retries.

### Asynchronous usage

Async usage is identical to the synchronous examples:

```csharp
var maxRetries = 10;
var retryEvery = TimeSpan.FromSeconds(5);
await Retry.DoAsync(async () =>
{
    await DoMyNetworkOperationAsync();
}, retryEvery, maxRetries);
```

### Advanced tips

If you need access to the retry information, you can specify access as follows:

```csharp
var maxRetries = 10;
var retryEvery = TimeSpan.FromSeconds(5);
 Retry.Do((retryIteration, maxRetryCount) =>
{
    Console.WriteLine($"Retry #: {retryIteration}");
    Console.WriteLine($"MaxRetries #: {maxRetryCount}");
    DoMyNetworkOperation();
}, retryEvery, maxRetries);
```

If you want to run some code when a retry fails, use the onFailure handler:
```csharp
var maxRetries = 10;
var retryEvery = TimeSpan.FromSeconds(5);
 Retry.Do(() =>
{
    DoMyNetworkOperation();
}, retryEvery, maxRetries, RetryPolicy.StaticDelay, RetryPolicyOptions.None, (retryIteration, maxRetryCount) => {
  // add your custom error code here
});
```

A rarely useful feature is being able to always retry if a condition is true, and if a condition is false it will retry up to the maximum number of retries. This can be useful if you want to always retry based on some condition like a status check, but if that condition isn't met to retry until failure.
```csharp
var maxRetries = 1;
var retryEvery = TimeSpan.FromMilliseconds(100);
var i = 0;
Retry.Do(() =>
{
    i++;
    DoMyNetworkOperation();
}, retryEvery, 
    maxRetries, 
    RetryPolicy.StaticDelay, 
    RetryPolicyOptions.None, 
    (retryIteration, maxRetryCount) => {},
    () => {
        // this must return true otherwise it will retry forever!
        // the result of this is it will retry until i is 10, no matter the maxRetries value.
        return i != 10;
    }
);
```

