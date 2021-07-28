using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicNotes
{
    float[] notes = {
        27.5f,
        29.13523509f,
        30.86770633f,
        32.70319566f,
        34.64782887f,
        36.70809599f,
        38.89087297f,
        41.20344461f,
        43.65352893f,
        46.24930284f,
        48.9994295f,
        51.9130872f,
        55f,
        58.27047019f,
        61.73541266f,
        65.40639133f,
        69.29565774f,
        73.41619198f,
        77.78174593f,
        82.40688923f,
        87.30705786f,
        92.49860568f,
        97.998859f,
        103.8261744f,
        110f,
        116.5409404f,
        123.4708253f,
        130.8127827f,
        138.5913155f,
        146.832384f,
        155.5634919f,
        164.8137785f,
        174.6141157f,
        184.9972114f,
        195.997718f,
        207.6523488f,
        220f,
        233.0818808f,
        246.9416506f,
        261.6255653f,
        277.182631f,
        293.6647679f,
        311.1269837f,
        329.6275569f,
        349.2282314f,
        369.9944227f,
        391.995436f,
        415.3046976f,
        440f,
        466.1637615f,
        493.8833013f,
        523.2511306f,
        554.365262f,
        587.3295358f,
        622.2539674f,
        659.2551138f,
        698.4564629f,
        739.9888454f,
        783.990872f,
        830.6093952f,
        880f,
        932.327523f,
        987.7666025f,
        1046.502261f,
        1108.730524f,
        1174.659072f,
        1244.507935f,
        1318.510228f,
        1396.912926f,
        1479.977691f,
        1567.981744f,
        1661.21879f,
        1760f,
        1864.655046f,
        1975.533205f,
        2093.004522f,
        2217.461048f,
        2349.318143f,
        2489.01587f,
        2637.020455f,
        2793.825851f,
        2959.955382f,
        3135.963488f,
        3322.437581f,
        3520f,
        3729.310092f,
        3951.06641f,
        4186.009045f


        };
    public float GetNote(int i)
    {
        if (i < 0 || i > 87) { return notes[48]; }
        else { return notes[i]; }
    }
}
