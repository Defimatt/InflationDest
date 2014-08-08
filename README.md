# InflationDest - easy Stellar voting for Windows

Stellar InflationDest is a quick and dirty tool that allows you to view the `InflationDest` of any account, and to set the `InflationDest` of accounts you own.

By doing this, you change the account your Stellars vote for to receive inflation (see https://www.stellar.org/about/mandate/#Stellar_creation for details).

Getting an `InflationDest` only requires an address. However, setting an `InflationDest` requires an address and a secret key. Unfortunately, there is not much I can do to convince you that this program doesn't do anything nasty with your secret key. However, I have:

* Created this program in .NET so that it can be reflected or decompiled using something like JetBrains dotPeek to see what the code is doing
* Shared the source here on GitHub so you can build it from source if you don't trust the precompiled binaries.

If you have an a username (e.g. Duffles) but don't have the long address, you can lookup the address. However, the federation server that supplies this information was running intermittently at the time of writing, so be patient with it. If I have time, I will extend this to include other features, such as creating new accounts without using the web client and sending Stellar. Have fun!

### Attributions:

* ModernUI: https://mui.codeplex.com/
* Json.NET: http://json.codeplex.com/
* Rocket by the Noun Project: http://www.thexamlproject.com/#/artwork/89

### Instructions:

Should be fairly simple to work out, but there's a quick YouTube explanation here: https://www.youtube.com/watch?v=OPptZZjVcTk

### Tips:

Tips gratefully received!
* Stellar: gDUNatU6tFPgkgW58U5uNf41ArMDCPD6XV
* Bitcoin: 1PAYMATT1UMKDPuHfqvwTmJuEtuYt7BspQ

### Where should I point my InflationDest?

See the following for ideas:
* https://stellartalk.org/topic/74-stellar-creation-and-voting/
* https://stellartalk.org/topic/385-join-str-mining-pool/