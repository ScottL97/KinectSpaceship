namespace Assets.Scripts.Game
{
    public class Planet
    {
        private string name; //行星名称
        private double radius; //赤道半径
        private double escape_velocity; //逃逸速度
        private double mass; //质量
        private double density; //密度
        private double revolution_period; //公转周期
        private double rotation_period; //自转周期
        
        public Planet(string name)
        {
            this.name = name;
        }

        public string GetName()
        {
            return name;
        }
        public double GetRadius()
        {
            return radius;
        }
        public double GetEscapeVelocity()
        {
            return escape_velocity;
        }
        public double GetMass()
        {
            return mass;
        }
        public double GetDensity()
        {
            return density;
        }
        public double GetRevolutionPeriod()
        {
            return revolution_period;
        }
        public double GetRotationPeriod()
        {
            return rotation_period;
        }
        public void SetRadius(double r)
        {
            radius = r;
        }
        public void SetEscapeVelocity(double v)
        {
            escape_velocity = v;
        }
        public void SetMass(double m)
        {
            mass = m;
        }
        public void SetDensity(double d)
        {
            density = d;
        }
        public void SetRevolutionPeriod(double rp)
        {
            revolution_period = rp;
        }
        public void SetRotationPeriod(double rp)
        {
            rotation_period = rp;
        }
    }
}
