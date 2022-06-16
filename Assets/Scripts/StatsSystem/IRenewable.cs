using System;

namespace StatsSystem
{
    public interface IRenewable
    {
        public event Action OnStatRenewed;
        void Renew();
    }
}