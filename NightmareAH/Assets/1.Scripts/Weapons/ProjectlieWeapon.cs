
public class ProjectlieWeapon : baseWeapon
{
    public float MaxRange;
    public float MinRange;
    public float Speed;
    public int PierceCount;
    public float PierceDamagePer;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="MaxRange">�ִ� �����Ÿ�</param>
    /// <param name="MinRange">�߻� �����Ÿ�</param>
    /// <param name="speed">����ü �ӵ�</param>
    /// <param name="pierceCount">���� Ƚ��</param>
    /// <param name="piercePer">���� ������ ���� �ۼ�Ʈ</param>
    /// <param name="Damage">������</param>
    /// <param name="frequency">�󵵼�</param>
    public ProjectlieWeapon(float MaxRange, float MinRange ,float speed , int pierceCount, float piercePer, float Damage, float frequency,int MaxLv, DataTableManager.WeaponIconData icon) : base(Damage, frequency,MaxLv,icon)
    {
        this.MaxRange = MaxRange;
        this.MinRange = MinRange;
        this.Speed = speed;
        this.PierceCount = pierceCount;
        this.PierceDamagePer = piercePer;
    }

    public override void Action()
    {

    }
    public override void LvUp()
    {
    }
}
