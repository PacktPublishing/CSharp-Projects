using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnigmaSimulator.Domain;

public class RotorAdvancer : IEnigmaModule
{
    public IEnigmaModule? NextModule { get; set; }

    public char Encode(char input, bool isForward) => input;

    public char Process(char input)
    {
        Queue<Rotor> rotorsToAdvance = BuildRotorAdvancementQueue();
        
        bool shouldAdvance = true;
        while (shouldAdvance && rotorsToAdvance.Count > 0)
        {
            Rotor rotor = rotorsToAdvance.Dequeue();
            shouldAdvance = rotor.Advance();
        }

        return NextModule?.Process(input) ?? input;
    }

    private Queue<Rotor> BuildRotorAdvancementQueue()
    {
        Queue<Rotor> rotors = new();
        IEnigmaModule? currentModule =  this;
        while (currentModule != null)
        {
            if (currentModule is Rotor rotor)
            {
                rotors.Enqueue(rotor);
            }
            currentModule = currentModule.NextModule;
        }
        
        return rotors;
    }
}