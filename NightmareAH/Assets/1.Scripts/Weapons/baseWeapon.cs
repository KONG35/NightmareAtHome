
public abstract class baseWeapon 
{

    public float Damage;
    public float Frequency;


    protected baseWeapon(float Damage,float frequency)
    {
        this.Damage = Damage;
        this.Frequency = frequency;
    }

    public abstract void Action();
}
