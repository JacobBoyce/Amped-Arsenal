using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[Serializable]
public class Stats : IEnumerable<Stat>
{
    [SerializeField]
    public Stats() => _stats = new List<Stat>();

        public void AddStat(Stat stat) => _stats.Add(stat);
        public void AddStat(string name, int value) => _stats.Add(new Stat {Name = name, Max = value, Value = value});
        public void AddStat(string name, int value,int max) => _stats.Add(new Stat {Name = name, Max = max, Value = value});

        [SerializeField]
        public Stat this[string name]
        {
            get => Find(name);
            set
            {
                var found = Find(name);
                if (found != null)
                {
                    found._statName = value.Name;
                    found.Value = value.Value;
                    found.Max = value.Max;
                }

            }
        }

        Stat Find(string name) =>_stats.FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        
        public IEnumerator<Stat> GetEnumerator() => _stats.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        [SerializeField]
        List<Stat> _stats;

        public void Fill()
        {
            foreach (var stat in _stats) stat.Fill();
        }
}
