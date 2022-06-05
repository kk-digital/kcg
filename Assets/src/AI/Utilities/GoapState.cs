using System;
using System.Linq;
using System.Collections.Generic;

namespace AI
{
    // Todo add Register state method.
    public class GoapState: IEquatable<GoapState>
    {
        // Todo use more efficient data structure.
        public Dictionary<string, object> states;

        public GoapState(Dictionary<string, object> other)
        {
            states = new Dictionary<string, object>(other);
        }

        public bool MatchCondition(GoapState other) // Check if states key exist inside other and if their value match.
        {
            foreach (var key in states.Keys)
            {
                if (!other.states.ContainsKey(key))
                {
                    return false;
                }

                if (!Equals(other.states[key], states[key]))
                {
                    return false;
                }
            }       
            return true;
        }

        public GoapState GetMissing(GoapState other)
        {
            GoapState MissingStates = new GoapState(new Dictionary<string, object>());
            foreach (var key in states.Keys)
            {
                if (!other.states.ContainsKey(key))
                {
                    continue;
                }

                if (Equals(other.states[key], states[key]))
                {
                    continue;
                }

                MissingStates.states.Add(key, states[key]);
            }
            return MissingStates;
        }

        public void ApplyEffect(GoapState Effects)
        {
            foreach (var key in Effects.states.Keys)
            {
                if(states.ContainsKey(key))
                    states[key] = Effects.states[key];
                else
                    states.Add(key, Effects.states[key]);
            }
        }

        static public GoapState ApplyEffect(GoapState States, GoapState Effects)
        {
            GoapState Other = new GoapState(States.states);
            foreach (var key in Effects.states.Keys)
            {
                if (Other.states.ContainsKey(key))
                    Other.states[key] = Effects.states[key];
                else
                    Other.states.Add(key, Effects.states[key]);
            }
            return Other;
        }

        public bool Equals(GoapState Other)
        {
            if (this.states.Count != Other.states.Count)
            {
                return false;
            }
            if (this.states.Keys.Except(Other.states.Keys).Any())
            {
                return false;
            }
            foreach (var state in Other.states)
            {
                if (!Equals(state.Value, this.states[state.Key]))
                    return false;
            }
            return true;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            else
                return Equals((GoapState)obj);
        }

        public static bool operator == (GoapState g1, GoapState g2)
        {
            return g1.Equals(g2);
        }

        public static bool operator !=(GoapState g1, GoapState g2)
        {
            return !g1.Equals(g2);
        }

        public override int GetHashCode()
        {
            return states.GetHashCode();
        }
    }
}
