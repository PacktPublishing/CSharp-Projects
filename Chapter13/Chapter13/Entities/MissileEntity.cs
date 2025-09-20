namespace Chapter13.Entities;

public class MissileEntity : SpaceEntityBase
{
    public override float MaxSpeed => 25f;
    public override float MaxTurnRate => 0.75f;
    public override float DetectionRadius => 25f;
}