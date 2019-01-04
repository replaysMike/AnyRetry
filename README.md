# AnyRetry
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

There are 3 types of policies available: `StaticDelay`, `ExponentialBackoff`, `EasedBackoff`. 

StaticDelay specifies that the retry time is always the same. ExponentialBackoff will multiply the `retryEvery` exponentially on every failure (a maximum can also be specified). EasedBackoff allows you to choose a standard easing algorithm, such as `ExponentialEaseOut` or `QuadraticEaseOut` for example.
