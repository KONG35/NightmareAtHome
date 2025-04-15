
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
    /// <param name="MaxRange">최대 사정거리</param>
    /// <param name="MinRange">발사 사정거리</param>
    /// <param name="speed">투사체 속도</param>
    /// <param name="pierceCount">관통 횟수</param>
    /// <param name="piercePer">관통 데미지 변경 퍼센트</param>
    /// <param name="Damage">데미지</param>
    /// <param name="frequency">빈도수</param>
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
