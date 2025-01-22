using System;
using NBitcoin;
using NBXplorer.DerivationStrategy;
using Xunit;
using Xunit.Abstractions;

namespace NBXplorer.Tests.Liquid;

public class LiquidTests
{
	public LiquidTests(ITestOutputHelper helper)
	{
		Logs.Tester = new XUnitLog(helper) { Name = "Tests" };
		Logs.LogProvider = new XUnitLoggerProvider(helper);
	}
	NBXplorerNetworkProvider _Provider = new NBXplorerNetworkProvider(ChainName.Mainnet);
	private NBXplorerNetwork GetNetwork(INetworkSet network)
	{
		return _Provider.GetFromCryptoCode(network.CryptoCode);
	}
	
	[Fact]
	public void TestingLiquidDerivation()
	{
		var network = GetNetwork(NBitcoin.Altcoins.AltNetworkSets.Liquid);
		// var x = new ExtKey().Neuter().GetWif(network.NBitcoinNetwork);
		// var plainXpub = network.DerivationStrategyFactory.Parse($"{x}");
		var strategy = network.DerivationStrategyFactory.Parse(
			$"xpub6BemYiVNp19a1d9vYWSkqf2cMnut2V3jF9Tmcht2y4zpLefRYXLw5tc5QjjGGDFfgHtCT2DCVxVY569281dbuhWEgpwdBU97Uv83PtcbtzK-[p2sh]-[slip77=e2eb4f6a06282febfb3c1fb3fe58fa73c833c564b8810e252a9fe9582454d4e4]");
		
		var deposit = new KeyPathTemplates(null).GetKeyPathTemplate(DerivationFeature.Deposit);
		var line = strategy.GetLineFor(deposit);
		for (uint i = 0; i < 10; i++)
		{
			// var keyPath = deposit.GetKeyPath(i);
			// var rootedKeyPath = RootedKeyPath.Parse("57b3f43a/84'/1'/0'");
			var derivation = line.Derive(i);
			var address = network.CreateAddress(strategy,
				line.KeyPathTemplate.GetKeyPath(i),
				derivation.ScriptPubKey).ToString();
			
			Logs.Tester.LogInformation(address);
		}
		
		// Generates
		// VJLGrnCWkY6jthoKCQse21zMNJTPBiNjtLbPUAbW44xppF4w7HsGaBasn8ksQKH2o8sQAGz9LivYuL5F
		// VJL9jrYSni1JjVFUDnMHRKD6hQofHQMYb6M44dogJBawwJDCWn6hYDz9aThopf2cGr1QpFPH4fEyYPYe
		// VJLB3tZH15yi6D72Nix6MRTPwP8hQwMP7iRtA5npgpfJ9HUqxZgWcYmUnMPCZwFCXDYh6ywGqBBX979i
		// ...
	}
}