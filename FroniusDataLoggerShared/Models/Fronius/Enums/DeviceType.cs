﻿namespace FroniusDataLoggerShared.Models
{
    /// <summary>
    /// Possible devices types to query
    /// </summary>
    public enum DeviceType
    {
        // 0x00 (0), interfacecard
        InterfaceCard = 0,
        // 0x01 (1), inverter
        Inverter = 1,
        // 0x02 (2), Sensor Card
        SensorCard = 2,


        //0x88 (136); Fronius IG 50;
        IG50 = 136,
        //0xD0 (208); Fronius IG Plus 50;
        IGPlus50 = 208,
        //0xD1 (209); Fronius IG Plus 100;
        IGPlus100 = 209,
        //0xD2 (210); Fronius IG Plus 100;
        IGPlus100_2 = 210,
        //0xD3 (211); Fronius IG Plus 150;
        IGPlus150 = 211,
        //0xD4 (212); Fronius IG Plus 35
        IGPlus35 = 212,
        //0xD5 (213); Fronius IG Plus 70;
        IGPlus70 = 213,
        //0xD6 (214); Fronius IG Plus 70;
        IGPlus70_2 = 214,
        //0xD7 (215); Fronius IG Plus 120;
        IGPlus120 = 215,
        //0xF9 (249); Fronius IG 60 HV;
        IG60HV = 249,
        //0xF3 (243); Fronius IG 60 ADV;
        IG60ADV = 243,
        //0xFA (250); Fronius IG 40;
        IG40 = 250,
        //0xFB (251); Fronius IG 30 Dummy;
        IG30Dummy = 251,
        //0xFC (252); Fronius IG 30;
        IG30 = 252,
        //0xFD (253); Fronius IG 20;
        IG20 = 253,
        //0xFE (254); Fronius IG 15;
        IG15 = 254,

        // Not yet set placeholder
        NotSet = 255,

            
    }
}
//        Type; Official; Device family
//0xBE (190); Fronius IG TL 3.0; IG TL
//0xC1 (193); Fronius IG TL 3.6; IG TL
//0xBF (191); Fronius IG TL 4.0; IG TL
//0xC3 (195); Fronius IG TL 4.6; IG TL
//0xC0 (192); Fronius IG TL 5.0; IG TL
//0xC2 (194); Fronius IG TL Dummy; IG TL
//0xBC (188); Fronius CL 36.0;
//0xBD (189); Fronius CL 48.0;
//0xC9 (201); Fronius CL 60.0;
//0xF6 (246); Fronius IG 300;
//0xF5 (245); Fronius IG 400;
//0xF4 (244); Fronius IG 500;
//0xEE (238); Fronius IG 2000;
//0xED (237); Fronius IG 3000;
//0xE5 (229); Fronius IG 2500-LV;
//0xEB (235); Fronius IG 4000;
//0xEA (234); Fronius IG 5100;
//0xE3 (227); Fronius IG 4500-LV;
//0xD8 (216); Fronius IG Plus 3.0-1 UNI;
//0xD9 (217); Fronius IG Plus 3.8-1 UNI;
//0xDA (218); Fronius IG Plus 5.0-1 UNI;
//0xDB (219); Fronius IG Plus 6.0-1 UNI;
//0xDC (220); Fronius IG Plus 7.5-1 UNI;
//0xDD (221); Fronius IG Plus 10.0-1 UNI;
//0xDE (222); Fronius IG Plus 11.4-1 UNI;
//0xDF (223); Fronius IG Plus 11.4-3 Delta;
//0xCF (207); Fronius IG Plus 12.0-3 WYE277;
//0xB9 (185); Fronius CL 36.0 WYE277;
//0xBA (186); Fronius CL 48.0 WYE277;
//0xBB (187); Fronius CL 60.0 WYE277;
//0xB6 (182); Fronius CL 33.3 Delta;
//0xB7 (183); Fronius CL 44.4 Delta;
//0xB8 (184); Fronius CL 55.5 Delta;
//0xC4 (196); SPR 12000F EU;
//0xB5 (181); SPR 10000F EU;
//0xC5 (197); SPR 8000F EU;
//0xC6 (198); SPR 6500F EU;
//0xC7 (199); SPR 4000F EU;
//0xC8 (200); SPR 3300F EU;
//0xB3 (179); SPR 12000f-277;
//0xCA (202); SPR 12000f;
//0xB2 (178); SPR 11400f-3 208/240;
//0xB4 (180); SPR 10000f;
//0xCB (203); SPR 8000f;
//0xCC (204); SPR 6500f;
//0xCD (205); SPR 4000f;
//0xCE (206); SPR 3300f;
//0x89 (137); Fronius IG Plus 30 V-1;
//0xB1 (177); Fronius IG Plus 35 V-1;
//0xB0 (176); Fronius IG Plus 50 V-1;
//0xAF (175); Fronius IG Plus 70 V-1;
//0xAE (174); Fronius IG Plus 70 V-2;
//0xAD (173); Fronius IG Plus 100 V-1;
//0xAC (172); Fronius IG Plus 100 V-2;
//0x84 (132); Fronius IG Plus 100 V-3;
//0xAB (171); Fronius IG Plus 120 V-3;
//0xAA (170); Fronius IG Plus 150 V-3;
//0xA9 (169); Fronius IG Plus V/A 3.0-1 UNI;
//0xA8 (168); Fronius IG Plus V/A 3.8-1 UNI;
//0xA7 (167); Fronius IG Plus V/A 5.0-1 UNI;
//0xA6 (166); Fronius IG Plus V/A 6.0-1 UNI;
//0xA5 (165); Fronius IG Plus V/A 7.5-1 UNI;
//0xA4 (164); Fronius IG Plus V/A 10.0-1 UNI;
//0xA3 (163); Fronius IG Plus V/A 11.4-1 UNI;
//0x87 (135); Fronius IG Plus V/A 10.0-3 Delta;
//0xA2 (162); Fronius IG Plus V/A 11.4-3 Delta;
//0xA1 (161); Fronius IG Plus V/A 12.0-3 WYE;
//0xA0 (160); Fronius IG Plus 50 V-1 Dummy;
//0x9F (159); Fronius IG Plus 100 V-2 Dummy;
//0x9E (158); Fronius IG Plus 150 V-3 Dummy;
//0x9D (157); Fronius IG Plus V 3.8-1 Dummy;
//0x9C (156); Fronius IG Plus V 7.5-1 Dummy;
//0x9B (155); Fronius IG Plus V 12.0-3 Dummy;
//0x9A (154); Fronius CL 60.0 Dummy;
//0x99 (153); Fronius CL 55.5 Delta Dummy;
//0x98 (152); Fronius CL 60.0 WYE277 Dummy;
//0x86 (134); SPR 3001F-1 EU;
//0x97 (151); SPR 3501F-1 EU;
//0x96 (150); SPR 4001F-1 EU;
//0x95 (149); SPR 6501F-2 EU;
//0x94 (148); SPR 8001F-2 EU;
//0x93 (147); SPR 10001F-3 EU;
//0x92 (146); SPR 12001F-3 EU;
//0x91 (145); SPR-3301f-1 UNI;
//0x90 (144); SPR-3801f-1 UNI;
//0x8F (143); SPR-6501f-1 UNI;
//0x8E (142); SPR-7501f-1 UNI;
//0x8D (141); SPR-10001f-1 UNI;
//0x8C (140); SPR-11401f-3 Delta;
//0x8B (139); SPR-12001f-3 WYE277;
//0x8A (138); SPR-11401f-1 UNI;
//0x85 (133); Fronius Agilo 100.0-3;
//0x83 (131); Fronius IG Plus 25 V-1;
//0x82 (130); SPR 8001F-3 EU;
//0x81 (129); Fronius IG Plus 60 V-1;
//0x80 (128); Fronius IG Plus 60 V-2;
//0xF8 (248); Fronius Galvo 3.1-1; Galvo
//0xF7 (247); Fronius Symo 3.0-3-S; Symo 4k5
//0xF2 (242); Fronius IG Plus 55 V-3;
//0xF1 (241); Fronius IG Plus 60 V-3;
//0xF0 (240); Fronius IG Plus 80 V-3;
//0xEF (239); Fronius Galvo 3.1-1 Dummy; Galvo Dummy
//0xEC (236); Fronius Symo 8.2-3-M Dummy; Symo 8k2 Multistring Dummy
//0xE9 (233); Fronius Symo 12.5-3-M; Symo 20k Multistring
//0xE8 (232); Fronius Symo 10.0-3-M; Symo 20k Multistring
//0xE7 (231); Fronius Agilo 100.0-3 Dummy;
//0xE6 (230); Fronius Agilo 75.0-3;
//0xE4 (228); Fronius Galvo 1.5-1; Galvo
//0xE2 (226); Fronius Galvo 2.0-1; Galvo
//0xE1 (225); Fronius Galvo 2.5-1; Galvo
//0xE0 (224); Fronius Galvo 3.0-1; Galvo
//0x7F (127); Fronius Symo 3.7-3-S; Symo 4k5
//0x7E (126); Fronius Symo 4.5-3-S; Symo 4k5
//0x7D (125); Fronius Symo 5.5-3-M; Symo 8k2 Multistring
//0x7C (124); Fronius Symo 6.7-3-M; Symo 8k2 Multistring
//0x7B (123); Fronius Symo 8.2-3-M; Symo 8k2 Multistring
//0x7A (122); Fronius Symo 5.0-3-M; Symo 8k2 Multistring
//0x79 (121); Fronius Symo 20.0-3-M; Symo 20k Multistring
//0x78 (120); Fronius Symo 20.0-3 Dummy; Symo 20k Multistring Dummy
//0x77 (119); Fronius IG Plus 55 V-2;
//0x76 (118); Fronius IG Plus 55 V-1;
//0x75 (117); Fronius Agilo 100.0-3 Outdoor;
//0x74 (116); Fronius Agilo 75.0-3 Outdoor;
//0x73 (115); Fronius Symo 15.0-3-M; Symo 20k Multistring
//0x72 (114); Fronius Symo 17.5-3-M; Symo 20k Multistring
//0x71 (113); Fronius Symo 3.0-3-M; Symo 8k2 Multistring
//0x70 (112); Fronius Symo 3.7-3-M; Symo 8k2 Multistring
//0x6F (111); Fronius Symo 4.5-3-M; Symo 8k2 Multistring
//0x6E (110); Fronius Symo 6.0-3-M; Symo 8k2 Multistring
//0x6D (109); Fronius Galvo 1.5-1 208-240; Galvo
//0x6C (108); Fronius Galvo 2.0-1 208-240; Galvo
//0x6B (107); Fronius Galvo 2.5-1 208-240; Galvo
//0x6A (106); Fronius Galvo 3.1-1 208-240; Galvo
//0x69 (105); Fronius Symo 7.0-3-M;
//0x68 (104); Fronius Agilo TL 460.0-3;
//0x67 (103); Fronius Agilo TL 360.0-3;
//0x66 (102); Fronius Primo 8.2-1; Primo ROW
//0x65 (101); Fronius Primo 8.2-1 208-240; Primo US
//0x64 (100); Fronius Primo 8.2-1 Dummy; Primo ROW
//0x63 (99); Fronius Symo Hybrid 5.0-3-S; Symo Hybrid
//0x62 (98); Fronius Symo 10.0-3 208-240; Symo 20k Multistring US 208-240
//0x61 (97); Fronius Symo 12.0-3 208-240; Symo 20k Multistring US 208-240
//0x60 (96); Fronius Symo 10.0-3 480; Symo 20k Multistring US 480
//0x5F (95); Fronius Symo 12.5-3 480; Symo 20k Multistring US 480
//0x5E (94); Fronius Symo 15.0-3 480; Symo 20k Multistring US 480
//0x5D (93); Fronius Symo 17.5-3 480; Symo 20k Multistring US 480
//0x5C (92); Fronius Symo 20.0-3 480; Symo 20k Multistring US 480
//0x5B (91); Fronius Symo 22.7-3 480; Symo 20k Multistring US 480
//0x5A (90); Fronius Symo 24.0-3 480; Symo 20k Multistring US 480
//0x59 (89); Fronius Symo 24.0-3 USA Dummy; Symo 20k Multistring US Dummy
//0x58 (88); Fronius Primo 7.6-1 208-240; Primo US
//0x57 (87); Fronius Primo 6.0-1 208-240; Primo US
//0x56 (86); Fronius Primo 5.0-1 208-240; Primo US
//0x55 (85); Fronius Primo 3.8-1 208-240; Primo US
//0x54 (84); Fronius IG Plus 120 V-1; IG Plus
//0x53 (83); Fronius Symo Hybrid 3.0-3-S; Symo Hybrid
//0x52 (82); Fronius Symo Hybrid 4.0-3-S; Symo Hybrid
//0x51 (81); Fronius Primo 3.0-1; Primo ROW
//0x50 (80); Fronius Primo 3.5-1; Primo ROW
//0x4F (79); Fronius Primo 3.6-1; Primo ROW
//0x4E (78); Fronius Primo 4.0-1; Primo ROW
//0x4D (77); Fronius Primo 4.6-1; Primo ROW
//0x4C (76); Fronius Primo 5.0-1; Primo ROW
//0x4B (75); Fronius Primo 6.0-1; Primo ROW
//0x4A (74); Remote Plant;
//0x49 (73); FRONIUS Eco 25.0-3-S; Eco
//0x48 (72); FRONIUS Eco 27.0-3-S; Eco
//0x47 (71); FRONIUS Symo 15.0-3 208; Eco US 208-220