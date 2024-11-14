 String[] ips = new string[] { "192.13.21.1","" };
      var  ipForwardChainHeaders = new StringValues(ips);
        var currentChain = ipForwardChainHeaders
                            .Where(v => !string.IsNullOrWhiteSpace(v))
                            .Select(IPAddress.Parse!)
                            .ToList();
