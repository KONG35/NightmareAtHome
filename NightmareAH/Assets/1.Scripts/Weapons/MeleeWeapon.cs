
public class MeleeWeapon : baseWeapon
{
    public float Range;

    public MeleeWeapon(float Range,float Damage,float frequency) : base(Damage, frequency)
    {
        this.Range = Range;
    }

    public override void Action()
    {

    }
}
