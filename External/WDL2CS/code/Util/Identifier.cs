using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace WDL2CS
{
    class Identifier
    {
        static private List<Regex> s_regex = new List<Regex>();

        enum RegexID : int
        {
            Event = 0,
            Global = 1,
            Asset = 2,
            Object = 3,
            //Function = 4,
            //Math = 5,
            Flag = 6,
            Property = 7,
            Command = 8,
            List = 9,
            Skill = 10,
            Synonym = 11,
            SynonymLocal = 12,
            Global_Property = 13,
            Event_Property = 14,
            Object_Flag = 15,
            Math_Command = 16,
            Math_Skill_Property = 17,
            Synonym_Flag = 18,
            Skill_Property = 19,
            Command_Flag = 20,
            Global_Synonym_Property = 21,
            Command_Property = 22,
            Skill_Flag = 23,
        }

        static RegexID[] s_events = new RegexID[] { RegexID.Event, RegexID.Event_Property };
        static RegexID[] s_globals = new RegexID[] { RegexID.Global, RegexID.Global_Property, RegexID.Global_Synonym_Property };
        static RegexID[] s_flags = new RegexID[] { RegexID.Flag, RegexID.Object_Flag, RegexID.Synonym_Flag, RegexID.Command_Flag, RegexID.Skill_Flag};
        static RegexID[] s_properties = new RegexID[] { RegexID.Property, RegexID.Asset, RegexID.Object, RegexID.Global_Property, RegexID.Event_Property, RegexID.Math_Skill_Property, RegexID.Skill_Property, RegexID.Global_Synonym_Property, RegexID.Command_Property };
        static RegexID[] s_commands = new RegexID[] { RegexID.Command, RegexID.Math_Command, RegexID.Command_Flag, RegexID.Command_Property};
        static RegexID[] s_skills = new RegexID[] { RegexID.Skill , RegexID.Math_Skill_Property , RegexID.Skill_Property, RegexID.Skill_Flag }; 
        static RegexID[] s_synonyms = new RegexID[] { RegexID.Synonym, RegexID.Synonym_Flag, RegexID.Global_Synonym_Property };
        static RegexID[] s_localSynonyms = new RegexID[] { RegexID.SynonymLocal };
        static RegexID[] s_list = new RegexID[] { RegexID.List };

        static Identifier()
        {
            /*  0 */s_regex.Add(new Regex("\\G((?I)(IF_(AE|ALT|ANYKEY|APO|BKSL|BKSP|BRACKL|BRACKR|CAL|CAR|COMMA|CTRL|CUD|CUL|CUR|CUU|DEL|END|EQUALS|ESC|HOME|INS|JOY4|LEFT|LOAD|MIDDLE|MINUS|MSTOP|OE|PAUSE|PERIOD|PGDN|PGUP|PLUS|RIGHT|SEMIC|SLASH|SPACE|START|SZ|TAB|TAST|UE|F(1[0-2]|[1-9])|[0-9A-Z])))"));
            /*  1 */s_regex.Add(new Regex("\\G((?I)(EACH_SEC|LAYERS|MESSAGES|PANELS|VIDEO|NEXUS|LIGHT_ANGLE|IBANK|DRUMBANK|MIDI_PITCH|BIND|MAPFILE|SAVEDIR|PATH|DITHER|SAVE_KEYS|REMOTE_KEYS))"));
            /*  2 */s_regex.Add(new Regex("\\G((?I)(MODEL|SOUND|MUSIC|FLIC|BMAP|OVLY|FONT))"));
            /*  3 */s_regex.Add(new Regex("\\G((?I)(OVERLAY|PANEL|PALETTE|REGION|SKILL|STRING|SYNONYM|TEXTURE|TEXT|VIEW|WALL|WAY))"));
            /*  4 */s_regex.Add(new Regex("\\G((?I)(ACTION|RULES))"));//remove?
            /*  5 */s_regex.Add(new Regex("\\G((?I)(ACOS|COS|ATAN|TAN|SIGN|INT|EXP|LOG10|LOG2|LOG))"));//remove?
            /*  6 */s_regex.Add(new Regex("\\G((?I)(FLOOR_DESCEND|CEIL_DESCEND|FLOOR_ASCEND|FLOOR_LIFTED|CEIL_ASCEND|CEIL_LIFTED|TRANSPARENT|CANDELABER|DIAPHANOUS|IMMATERIAL|IMPASSABLE|PORTCULLIS|AUTORANGE|CAREFULLY|CONDENSED|INVISIBLE|SENSITIVE|BERKELEY|CENTER_X|CENTER_Y|LIGHTMAP|PASSABLE|SAVE_ALL|CLUSTER|CURTAIN|FRAGILE|NO_CLIP|ONESHOT|REFRESH|VISIBLE|ABSPOS|BEHIND|GROUND|LIFTED|MASTER|NARROW|RELPOS|SHADOW|STICKY|FENCE|GHOST|LIBER|MOVED|SLOOP|BASE|BLUR|CLIP|FLAT|HARD|PLAY|SEEN|WIRE|FAR|SKY))"));
            /*  7 */s_regex.Add(new Regex("\\G((?I)(ALBEDO|ANGLE|ASPEED|ATTACH|BELOW|BMAPS|BUTTON|CEIL_(ANGLE|OFFS_[X-Y]|TEX)|CYCLES|CYCLE|DEFAULT|DELAY|DIGITS|DISTANCE|DIST|EACH_CYCLE_(C|F)|EACH_CYCLE|FLAGS|FLOOR_(ANGLE|OFFS_[X-Y]|TEX)|FOOT_HGT|FRAME|GENIUS|HBAR|HEIGHT|HSLIDER|IF_(ARISE|ARRIVED|DIVE|FAR|LEAVE|NEAR|RELEASE|TOUCH)|INDEX|LAYER|LEFT|LENGTH|MAP_COLOR|MASK|MAX|MIN|MIRROR|OFFSET_[X-Y]|OVLYS|PALFILE|PAN_MAP|PICTURE|POSITION|POS_[X-Y]|RADIANCE|RANGE|REL_(ANGLE|DIST)|RIGHT|SCALE_(XY|X|Y)|SCYCLES|SCYCLE|SCALE|SDIST|SIDES|SIDE|SIZE_[X-Y]|SKILL[1-8]|SPEED|STRINGS|SVDIST|SVOL|TARGET_(MAP|X|Y)|TARGET|TEXTURE[1-4]|THING_HGT|TITLE|TOP|TOUCH|TYPE|VAL|VBAR|VSLIDER|VSPEED|WAYPOINT|WINDOW|[X-Z][1-2]|[X-Z]))"));
            /*  8 */s_regex.Add(new Regex("\\G((?I)(ACCEL|ADD_STRING|ADDT|ADD|AND|BEEP|BRANCH|BREAK|CALL|DROP|END|EXCLUSIVE|EXEC_RULES|EXIT|EXPLODE|FADE_PAL|FIND|FREEZE|GETMIDI|GOTO|IF_(ABOVE|BELOW|EQUAL|MAX|MIN|NEQUAL)|INKEY|INPORT|LEVEL|LIFT|LOAD_INFO|LOAD|LOCATE|MAP|MIDI_COM|NEXT_(MY_THERE|MY|THERE)|NOP|OUTPORT|PLACE|PLAY_(CD|DEMO|FLICFILE|FLIC|SONG_ONCE|SONG|SOUNDFILE|SOUND)|PRINTFILE|PRINT_(STRING|VALUE)|PUSH|RANDOMIZE|ROTATE|SAVE_(DEMO|INFO)|SCAN|SCREENSHOT|SETMIDI|SET_(ALL|INFO|SKILL|STRING)|SET|SHAKE|SHIFT|SHOOT|SKIP|STOP_(DEMO|FLIC|SOUND)|SUBT|SUB|TILT|TO_STRING|WAITT|WAIT_TICKS|WAIT))"));
            /*  9 */s_regex.Add(new Regex("\\G((?I)((EACH_TICK|EACH_SEC|PANELS|LAYERS|MESSAGES)\\.(1[0-6]|[1-9])))"));
            /* 10 */s_regex.Add(new Regex("\\G((?I)(ACCELERATION|ACTIONS|ACTIVE_(NEXUS|OBJTICKS|TARGETS)|ACTOR_(CEIL_HGT|CLIMB|DIST|FLOOR_HGT|IMPACT_V[X-Z]|WIDTH)|APPEND_MODE|ASPECT|BLUR_MODE|BOUNCE_V[X-Y]|CDAUDIO_VOL|CD_TRACK|CHANNEL_[0-7]|CHANNEL|CLIPPED|CLIPPING|COLOR_(ACTORS|BORDER|PLAYER|THINGS|WALLS)|DARK_DIST|DEBUG_MODE|DELTA_ANGLE|ERROR|FLIC_FRAME|FLOOR_MODE|FORCE_(AHEAD|ROT|STRAFE|TILT|UP)|FRAME_COLOR|FRICTION|HALF_PI|HIT_(DIST|MINDIST|X|Y)|IMPACT_V(ROT|[X-Z])|INERTIA|INV_DIST|JOYSTICK_[X-Y]|JOY_(4|SENSE)|KEY_(ALT|ANY|APO|BKSL|BKSP|BRACKL|BRACKR|CAL|CAR|COMMA|CTRL|CUD|CUL|CUR|CUU|DEL|END|ENTER|EQUALS|ESC|HOME|INS|JOY4|MINUS|PAUSE|PERIOD|PGDN|PGUP|PLUS|SEMIC|SENSE|SHIFT|SLASH|SPACE|SZ|TAB|F(1[0-2]|[1-9])|[A-Z0-9])|LIGHT_DIST|LOAD_MODE|MAP_(CENTER[X-Y]|(EDGE_[X-Y][1-2])|LAYER|MAX[X-Y]|MIN[X-Y]|MODE|OFFS[X-Y]|ROT|SCALE)|MAX_DIST|MICKEY_[X-Y]|MINV_DIST|MOTION_BLUR|MOUSE_(ANGLE|CALM|LEFT|MIDDLE|MODE|MOVING|RIGHT|SENSE|TIME|X|Y)|MOVE_(ANGLE|MODE)|MUSIC_VOL|MY_(X[1-2]|Y[1-2]|Z[1-2]|X|Y)|PANEL_LAYER|PI|PLAYER_(ANGLE|ARC|CLIMB|COS|DEPTH|HGT|LAST_[X-Y]|LIGHT|MSIN|SIN|SIZE|SPEED|TILT|VROT|V[X-Z]|WIDTH|[X-Z])|PSOUND_(TONE|VOL)|REAL_SPEED|REMOTE_[0-1]|RENDER_MODE|SCREEN_(HGT|WIDTH|X|Y)|SECS|SHIFT_SENSE|SHOOT_(ANGLE|FAC|RANGE|SECTOR|X|Y)|SKIP_FRAMES|SKY_OFFS_[X-Y]|SLOPE_(AHEAD|SIDE|X|Y)|SOUND_VOL|SPANS|STEPS|STR_LEN|TEXT_LAYER|THING_(DIST|WIDTH)|TICKS|TIME_(ACTIONS|CLIPPING|CORR|DRAW|FAC|FBUFFER|SLICES|TARGETS|VERTICES)|TOUCH_(DIST|MODE|RANGE|STATE)|TWO_PI|WALK_(PERIOD|TIME)|WALK|WAVE_PERIOD|WAVE|PALANIM_DELAY))"));
            /* 11 */s_regex.Add(new Regex("\\G((?I)(HIT|TOUCH_TEXT|TOUCHED|TOUCH_TEX|TOUCH_REG|COMMAND_LINE|REGION[1-8]))"));
            /* 12 */s_regex.Add(new Regex("\\G((?I)(THERE|MY))"));
            /* 13 */s_regex.Add(new Regex("\\G((?I)EACH_TICK|CLIP_DIST)"));
            /* 14 */s_regex.Add(new Regex("\\G((?I)(IF_ENTER|IF_HIT|IF_KLICK))"));
            /* 15 */s_regex.Add(new Regex("\\G((?I)(THING|ACTOR))"));
            /* 16 */s_regex.Add(new Regex("\\G((?I)(SIN|ASIN|SQRT|ABS))"));
            /* 17 */s_regex.Add(new Regex("\\G((?I)RANDOM)"));
            /* 18 */s_regex.Add(new Regex("\\G((?I)HERE)"));
            /* 19 */s_regex.Add(new Regex("\\G((?I)(FLOOR_HGT|CEIL_HGT|AMBIENT|RESULT|NODE))"));
            /* 20 */s_regex.Add(new Regex("\\G((?I)SAVE)"));
            /* 21 */s_regex.Add(new Regex("\\G((?I)MSPRITE)"));
            /* 22 */s_regex.Add(new Regex("\\G((?I)DO)"));
            /* 23 */s_regex.Add(new Regex("\\G((?I)(FLAG[1-8]))"));
            Regex.CacheSize += s_regex.Count;
        }

        public static bool IsEvent(ref string identifier)
        {
            return Is(s_events, ref identifier);
        }

        public static bool IsGlobal(ref string identifier)
        {
            return Is(s_globals, ref identifier);
        }

        public static bool IsMath(ref string identifier)
        {
            return false;
        }

        public static bool IsFlag(ref string identifier)
        {
            return Is(s_flags, ref identifier);
        }

        public static bool IsProperty(ref string identifier)
        {
            return Is(s_properties, ref identifier);
        }

        public static bool IsCommand(ref string identifier)
        {
            return Is(s_commands, ref identifier);
        }

        public static bool IsList(ref string identifier)
        {
            return Is(s_list, ref identifier);
        }

        public static bool IsSkill(ref string identifier)
        {
            return Is(s_skills, ref identifier);
        }

        public static bool IsSynonym(ref string identifier)
        {
            return Is(s_synonyms, ref identifier);
        }

        public static bool IsLocalSynonym(ref string identifier)
        {
            return Is(s_localSynonyms, ref identifier);
        }

        private static bool Is(RegexID[] lists, ref string identifier)
        {
            int index;
            Match m;
            for (int i = 0; i < lists.Length; i++)
            {
                index = (int)lists[i];
                m = s_regex[index].Match(identifier);
                if (m.Success && m.Length == identifier.Length)
                    return true;
            }
            return false;
        }
    }
}
