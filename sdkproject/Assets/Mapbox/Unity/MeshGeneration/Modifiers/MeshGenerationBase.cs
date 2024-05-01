using System.Collections.Generic;
using Mapbox.Unity.MeshGeneration.Modifiers;

public class MeshGenerationBase : MeshModifier, IReplaceable
{
	public HashSet<IReplacementCriteria> Criteria { get; set; }

	public override void Initialize()
	{
		base.Initialize();
		Criteria = new HashSet<IReplacementCriteria>();
	}
}
