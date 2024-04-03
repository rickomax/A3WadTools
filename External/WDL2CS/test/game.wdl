CLIP_DIST 200;
NEXUS 20;
#VIDEO x320x240;
VIDEO x320x400;
#VIDEO s640x480;
DITHER 0;
MAPFILE <area.wmp>;
BIND <game.wdl>;

STRING keydoor_str,"i need a key";

SOUND mselect_snd, <mselect.wav>;
SOUND menu_snd, <load1b.wav>;
SOUND donner_snd, <donner.wav>;

#SKILL SCREEN_WIDTH {VAL 320;}

SKILL levelnr {VAL 1;}
SKILL shop_itemA {}
SKILL shop_itemQ {}
SKILL q_amount {VAL 0;}
SKILL a_amount {VAL 0;}
SKILL title {VAL 1; TYPE PLAYER;}
SKILL BLUR_MODE {TYPE LOCAL; VAL 0;}
SKILL AMBIENT {VAL 0.2;}
SKILL player_light_s {VAL 0.8;}
SKILL HP {VAL 50; MAX 100; MIN 0;}
SKILL MP {VAL 0; MAX 50; MIN 0;}
SKILL option {VAL 1;}
SKILL heal {MIN 4;}
SKILL Magic_Arrow {VAL 0;}
SKILL Nightvision {VAL 0;}
SKILL Fireball {VAL 0;}
SKILL Fly {VAL 0;}
SKILL Shield {VAL 0;}
SKILL Lightning {VAL 0;}
SKILL Repair {VAL 0;}
SKILL Explosion {VAL 0;}
SKILL Town_Portal {VAL 0;}
SKILL normlight {}
SKILL night {VAL 0;}

SKILL use {VAL 0; MIN 0;}
SKILL fade {}
SKILL boomcount {} //only used for thunderstorm
SKILL titlecount {} //only used for intro restart
SKILL ACTOR_CLIMB {VAL 3;}
SKILL THING_WIDTH {VAL 0.5;}
SKILL ACTOR_WIDTH {VAL 0.5;}
SKILL LIGHT_DIST {VAL 20;}
SKILL PLAYER_LIGHT {VAL 0.8;}
SKILL MOTION_BLUR {TYPE LOCAL; VAL 0;}
SKILL SCREEN_HGT {VAL 326;}
SKILL MAP_EDGE_X1 {VAL 93;}
SKILL MAP_EDGE_X2 {VAL 227;}
SKILL MAP_EDGE_Y1 {VAL 22;}
SKILL MAP_EDGE_Y2 {VAL 321;}
SKILL MAP_LAYER {VAL 12;}
SKILL TEXT_LAYER {VAL 16;}
SKILL MAP_SCALE {VAL 1.5; MIN 0.5; MAX 15;}
SKILL COLOR_BORDER {VAL 9;}
SKILL COLOR_WALLS {VAL 7;}
SKILL COLOR_PLAYER {VAL 12;}
SKILL MUSIC_VOL {TYPE LOCAL; VAL 0.5; MIN 0; MAX 1;}
SKILL SOUND_VOL {TYPE LOCAL; VAL 0.5; MIN 0; MAX 1;}
SKILL switch1 {MAX 190; MIN 121;}
SKILL switch2 {MAX 190; MIN 121;}
SKILL switch3 {MAX 188; MIN 121;}
SKILL gamma {TYPE LOCAL; MAX 0.19; MIN 0; VAL 0;}
SKILL mapcheat {VAL 0;}
SKILL msg_timer {}
SKILL load_me {VAL 0; TYPE LOCAL;}
SKILL village {VAL 0; TYPE PLAYER;}
SKILL village_pos {VAL 0;}

SKILL lvl {VAL 1;}
SKILL int {VAL 3; MAX 99;}
SKILL spd {VAL 4; MAX 99;}
SKILL str {VAL 5; MAX 99;}
SKILL con {VAL 4; MAX 99;}
SKILL armor {VAL 0;}
SKILL AC {VAL 0;}
SKILL dmg {VAL 0;}
SKILL points {VAL 0; MAX 99999;}
SKILL nextlvl {VAL 19; MIN 0; MAX 99999;}
SKILL exp {VAL 19; MAX 99999;}
SKILL pts_left {VAL 0; MIN 0; MAX 7;}
SKILL new_HP {VAL 0; MIN 3;}
SKILL new_MP {VAL 0; MIN 1;}
SKILL weight {VAL 0; MAX 150.05;}
SKILL shotdmg {}
SKILL maxdmg {VAL 0;}
SKILL extradmg {VAL 0;}

SKILL gold {VAL 1000; MAX 99999;}
SKILL price {VAL 0;}
//////////Schattenskills//////////
SKILL HP_s {VAL 50; MAX 100; MIN 0;TYPE LOCAL;}
SKILL MP_s {VAL 0; MAX 100; MIN 0;TYPE LOCAL;}
SKILL heal_s {MIN 4; TYPE LOCAL;}
SKILL Magic_Arrow_s {VAL 0;TYPE LOCAL;}
SKILL Nightvision_s {VAL 0;TYPE LOCAL;}
SKILL Fireball_s {VAL 0;TYPE LOCAL;}
SKILL Fly_s {VAL 0;TYPE LOCAL;}
SKILL Shield_s {VAL 0;TYPE LOCAL;}
SKILL Lightning_s {VAL 0;TYPE LOCAL;}
SKILL Repair_s {VAL 0;TYPE LOCAL;}
SKILL Explosion_s {VAL 0;TYPE LOCAL;}
SKILL Town_Portal_s {VAL 0;TYPE LOCAL;}

SKILL lvl_s {VAL 1; TYPE LOCAL;}
SKILL int_s {VAL 5; MAX 99;TYPE LOCAL;}
SKILL spd_s {VAL 5; MAX 99;TYPE LOCAL;}
SKILL str_s {VAL 5; MAX 99;TYPE LOCAL;}
SKILL con_s {VAL 5; MAX 99;TYPE LOCAL;}
SKILL armor_s {VAL 0;TYPE LOCAL;}
SKILL AC_s {VAL 0;TYPE LOCAL;}
SKILL dmg_s {VAL 1;TYPE LOCAL;}
SKILL points_s {VAL 0; MAX 99999;TYPE LOCAL;}
SKILL nextlvl_s {VAL 10; MIN 0; MAX 99999;TYPE LOCAL;}
SKILL exp_s {VAL 10; MAX 99999;TYPE LOCAL;}
SKILL pts_left_s {VAL 0; MIN 0; MAX 7; TYPE LOCAL;}
SKILL weight_s {VAL 0; MAX 50;TYPE LOCAL;}
SKILL q_weigt_s {VAL 0; TYPE LOCAL;}
SKILL a_weigt_s {VAL 0; TYPE LOCAL;}
SKILL cur_wpn_s { VAL 0; TYPE LOCAL;}
SKILL free_item_s {VAL 2; MAX 21;TYPE LOCAL;}
SKILL cur_item_s {VAL 0;TYPE LOCAL;}
SKILL new_amount_s {VAL 0;TYPE LOCAL;}
SKILL cur_weight_s {VAL 0;TYPE LOCAL;}
SKILL new_weight_s {VAL 0;TYPE LOCAL;}
SKILL i_amount_s {VAL 0;TYPE LOCAL;}
SKILL i_weight_s {VAL 0;TYPE LOCAL;}
SKILL sel_item_s {VAL 1;TYPE LOCAL;}
SKILL tot_amount_s {VAL 0;TYPE LOCAL;}
SKILL q_amount_s {VAl 0;TYPE LOCAL;}
SKILL q_weight_s {VAL 0; TYPE LOCAL;}
SKILL a_amount_s {VAl 0;TYPE LOCAL;}
SKILL a_weight_s {VAL 0; TYPE LOCAL;}

SKILL i_item1_s {VAL 101;TYPE LOCAL;}
SKILL i_item2_s {VAL 0;TYPE LOCAL;}
SKILL i_item3_s {VAL 0;TYPE LOCAL;}
SKILL i_item4_s {VAL 0;TYPE LOCAL;}
SKILL i_item5_s {VAL 0;TYPE LOCAL;}
SKILL i_item6_s {VAL 0;TYPE LOCAL;}
SKILL i_item7_s {VAL 0;TYPE LOCAL;}
SKILL i_item8_s {VAL 0;TYPE LOCAL;}
SKILL i_item9_s {VAL 0;TYPE LOCAL;}
SKILL i_item10_s {VAL 0;TYPE LOCAL;}
SKILL i_item11_s {VAL 0;TYPE LOCAL;}
SKILL i_item12_s {VAL 0;TYPE LOCAL;}
SKILL i_item13_s {VAL 0;TYPE LOCAL;}
SKILL i_item14_s {VAL 0;TYPE LOCAL;}
SKILL i_item15_s {VAL 0;TYPE LOCAL;}
SKILL i_item16_s {VAL 0;TYPE LOCAL;}
SKILL i_item17_s {VAL 0;TYPE LOCAL;}
SKILL i_item18_s {VAL 0;TYPE LOCAL;}
SKILL i_item19_s {VAL 0;TYPE LOCAL;}
SKILL i_item20_s {VAL 0;TYPE LOCAL;}

SKILL h_item1_s {VAL 0;TYPE LOCAL;}
SKILL h_item2_s {VAL 0;TYPE LOCAL;}
SKILL h_item3_s {VAL 0;TYPE LOCAL;}
SKILL h_item4_s {VAL 0;TYPE LOCAL;}
SKILL h_item5_s {VAL 0;TYPE LOCAL;}
SKILL q_item_s {VAL 0;TYPE LOCAL;}
SKILL a_item_s {VAL 0;TYPE LOCAL;}

SKILL gold_s {VAL 0; TYPE LOCAL;}

SYNONYM NPC_ovr {TYPE OVERLAY;}
SYNONYM camera {TYPE THING;}
SYNONYM loop_sound {TYPE ACTION;}
#SYNONYM weapon_ovr {TYPE OVERLAY; DEFAULT club0_ovr;}
SYNONYM weapon_atk {TYPE ACTION; DEFAULT NULL;}
SKILL weapon_hgt {VAL 0;}

INCLUDE <strings.wdl>;
INCLUDE <overlays.wdl>;
INCLUDE <move.wdl>;
INCLUDE <gametex.wdl>;
#include <gametex2.wdl>;
include <monster.wdl>;
INCLUDE <invent.wdl>;
INCLUDE <buymenu.wdl>;
include <saveload.wdl>;
INCLUDE <cheats.wdl>;
include <debug.wdl>;
INCLUDE <items.wdl>;
INCLUDE <rob_reg.wdl>;
include <cave.wdl>;
include <swamp.wdl>;
include <cave2.wdl>;
INCLUDE <forest.wdl>;
include <area.wdl>;
include <arenas.wdl>;
include <village.wdl>;
INCLUDE <doors.wdl>;
INCLUDE <lifts.wdl>;

IBANK <gm.ibk>;
SOUND intro_snd, <storm.wav>;
PALETTE black_pal { PALFILE <black.bbm>;}

PALETTE pal1 {  PALFILE <testpal.pcx>;
	RANGE 16,32;
	RANGE 48,32; RANGE 80,32; RANGE 112,32;
	RANGE 144,32; RANGE 176,32; RANGE 208,32;
	#RANGE 240,15;
	FLAGS BLUR;
	}

IF_START startaction;

ACTION startaction {
	LOAD_INFO "WoK0",0;
	SET BLUR_MODE,0;
	SET SKY_OFFS_Y,55;
	CALL set_switches;
	CALL set_debug;
	SET RENDER_MODE,1;
	SET PLAYER_SIZE,7;
	SET my_size,6;
IF_NEQUAL title,1;
	SKIP 12;#10;
FADE_PAL black_pal,0.96;
set PANELS.10,logo_pan;
#	set PANELS.11,dragon_pan;
#	SET EACH_TICK.5,run_baby_run;
set title,0;
SET SCREEN_HGT,64;
SET MOVE_MODE,0;
SET RENDER_MODE,0;
CALL reset_controls;
SET IF_ESC,title_scr2;#IF_ANYKEY,title_scr;
PLAY_SOUND intro_snd,0.7;
SET fade,0.96;
SET EACH_TICK.8,fade_in;
END;
        SET PANELS.13,display_pan;
	SET disp_txt.VISIBLE,1;
}

//////////Aktionen//////////
ACTION restore_intro {SET EACH_TICK.8, restore_intro;
IF_EQUAL menu_txt.VISIBLE,1;CALL show_menu;
SET title,1; SET PANELS.10, logo_pan; 
SET PANELS.11,NULL;
#IF_NEQUAL PANELS.10, logo_pan; END;
#SET MY.EACH_TICK,NULL;
SET EACH_TICK.8,NULL;
SET EACH_TICK.5,NULL;
SET EACH_TICK.2,NULL;
SET MESSAGES.1,NULL;
SET titlecount,0;
BRANCH startaction;}

ACTION title_scr {
FADE_PAL nebel_pal,gamma;
SET EACH_TICK.2,title_text;
SET EACH_TICK.5,title_count;
SET EACH_TICK.8,title_ani;
SET PANELS.10,title_pan;
SET PANELS.11,NULL;
SET IF_ESC,NULL;
SET IF_ANYKEY,show_menu;
#PLAY_SOUNDFILE "theme.wav",1;
SAVE "WoK0",9;
}

ACTION title_scr2 {
SET PANELS.10,title_pan;
wait 1;
SET EACH_TICK.8, title_scr;
#PLAY_SOUNDFILE "theme.wav",1;
}

ACTION title_text {
SET MESSAGES.1,anykey_txt;
WAITT 24;
SET MESSAGES.1,NULL;
WAITT 12;
}

ACTION  title_ani {	
WAITT 74;
RULE lightning_pan.POS_X=300-(RANDOM*10*32);	
#RULE lightning_pan.POS_X=600-(RANDOM*10*64);	
SET PANELS.11,lightning_pan;
WAITT 2;
IF (gamma<0.1) {FADE_PAL nebel_pal, 0.1;}
WAITT 2; FADE_PAL nebel_pal,gamma;
WAITT 2;
SET PANELS.11,NULL;
RULE boomcount=INT(RANDOM*20+5);
WAITT boomcount;
RULE boomcount=1-((boomcount-5)/20);
PLAY_SOUND donner_snd,boomcount;
IF_BELOW RANDOM,0.6; END;
WAITT 64;
PLAY_SOUND intro_snd,0.2;
}	

ACTION title_count {
ADDT titlecount,1;
IF (titlecount>1000) {
SET messages.1,NULL;
SET IF_ANYKEY,NULL;
SET fade,0;
SET EACH_TICK.5, title_fade;
SET PANELS.11,NULL;
SET EACH_TICK.8,NULL;
SET EACH_TICK.2,NULL;
SET MESSAGES.1,NULL;}
}

ACTION title_fade {
IF_BELOW fade,2; SKIP 1;
BRANCH restore_intro;
IF_ABOVE FADE,0.9; SKIP 1;
FADE_PAL black_pal, fade;
ADD fade,0.06;
}


ACTION fade_in {
IF_ABOVE fade,-0.8; SKIP 1;
SET EACH_TICK.8,fade_out;
FADE_PAL black_pal, fade;
ADD fade,-0.04;
}

ACTION fade_in2 {
IF_ABOVE fade,-0.8; SKIP 1;
SET EACH_TICK.8,fade_out2;
FADE_PAL black_pal, fade;
ADD fade,-0.04;
}

ACTION fade_out {
IF_BELOW fade,0.88; SKIP 4;
set PANELS.10,woklogo_pan;
PLAY_SOUND intro_snd, 0.7;
SET EACH_TICK.8,fade_in2;
END;
FADE_PAL black_pal, fade;
ADD fade,0.04;
}

ACTION fade_out2 {
IF_ABOVE fade,0.8; SET PANELS.10, title_pan;
IF_BELOW fade,0.88; SKIP 2;
SET EACH_TICK.8,NULL;
BRANCH title_scr;
FADE_PAL black_pal, fade;
ADD fade,0.04;
}

ACTION show_map {
	IF_ABOVE MAP_MODE,0;
		GOTO pan_nomap;
	IF_BELOW levelnr,90; SKIP 3;
	SET newstring,1;
	SET info_str, nomap_str;
	END;
	SET MAP_MODE,0.5;
	IF_EQUAL mapcheat,1;
		SET MAP_MODE,1;
	SET mapborder_ovr.VISIBLE,1;
	END;
	pan_nomap:
	SET MAP_MODE,0;
	SET mapborder_ovr.VISIBLE,0;}

ACTION pause_game {
IF_EQUAL MOVE_MODE,-.5;
	SKIP 8;
SET MOVE_MODE,-.5;
CALL reset_controls;
CALL cheat_on;
SET IF_ENTER,pause_game;
SET IF_ESC,pause_game;
SET info_txt.STRING,pause_str;
SET info_txt.VISIBLE,1;
END;
SET info_txt.VISIBLE,NULL;	
CALL cheat_off;
CALL set_controls;
SET MOVE_MODE,1;}

ACTION use_item {SET use,1; PUSH 12; WAIT 1; SET use,0;}

ACTION show_conversation {
	IF_EQUAL LAYERS.3,inventory_ovr;
		CALL show_inventory;
	IF_EQUAL LAYERS.1,spellbook_ovr;//falls sichtbar, Zauberbuch schliessen
		CALL show_spells;
	IF_EQUAL LAYERS.2,character_ovr;//falls sichtbar, Charakterblatt schliessen
		CALL show_character;
SET newstring,1;
SET info_str,nix_str;
SET tilt,0; SET PLAYER_TILT,0;
SET qinvdata,LAYERS.5;
SET ainvdata,LAYERS.7;
SET LAYERS.5,NULL; SET LAYERS.7,NULL;
SET disp_txt.VISIBLE,0;
SET LAYERS.14,NULL;
SET PANELS.13,NULL;
SET NPC_ovr.VISIBLE,1;	
SET LAYERS.8,conversation_ovr;
SET PANELS.12, empty_pan;
SET conversation_txt.VISIBLE,1;
WAIT 1;
CALL stop_moving;
SET MOVE_MODE,0;
SET EACH_TICK.16,NULL;
CALL reset_controls;
}

ACTION hide_conversation {
SET disp_txt.VISIBLE,1;
SET LAYERS.5,qinvdata; SET LAYERS.7,ainvdata;
SET LAYERS.14,display2_ovr;
SET PANELS.13,display_pan;
SET conversation_txt.VISIBLE,NULL;
SET NPC_ovr.VISIBLE,0;
SET LAYERS.8,NULL;
SET PANELS.12,NULL;
CALL set_controls;
SET MOVE_MODE,1;
SET EACH_TICK.16,move_me;
CALL set_moving;
}

ACTION show_sign {
	WAIT 1;
	SET LAYERS.8,sign_ovr;
	SET sign_txt.VISIBLE,1;
	WAIT 1;
	CALL stop_moving;
	SET MOVE_MODE,0;
	SET EACH_TICK.16,NULL;
	CALL reset_controls;
	WAITT 4;
	SET IF_SPACE, hide_sign;
	SET IF_RIGHT, hide_sign;
}

ACTION hide_sign {
	SET sign_txt.VISIBLE,NULL;
	SET LAYERS.8,NULL;
	CALL set_controls;
	SET MOVE_MODE,1;
	SET EACH_TICK.16,move_me;
	CALL set_moving;
}

////////// Hilfe Anzeigen //////////
ACTION show_help {
IF_NEQUAL MESSAGES.15,help_txt;
	SKIP 13;
CALL darkening_off;
SET mapborder_ovr.VISIBLE,0;
SET MESSAGES.15,NULL;
SET IF_ESC,show_menu;
PLAY_SOUND mselect_snd,1;
IF_EQUAL PANELS.10,title_pan;
	SKIP 3;
CALL set_controls;
SET MOVE_MODE,1;
END;
CALL show_menu;
SET IF_ESC,NULL;
end;
PLAY_SOUND mselect_snd,1;
CALL darkening_on;
CALL reset_controls;
SET IF_CUR,helptext2;
SET help_txt.STRING,help_str;
SET IF_ESC,show_help;
SET IF_F1,show_help;
SET mapborder_ovr.VISIBLE,1;
SET MOVE_MODE,-0.5;
IF_NEQUAL PANELS.10,NULL; SET MOVE_MODE,0;
SET MAP_MODE,0;
SET MESSAGES.15,help_txt;}

ACTION helptext4 {
	PLAY_SOUND menu_snd,1;
	SET IF_CUR,NULL;
	SET IF_CUL,helptext3;
	SET help_txt.STRING,help_str4;}

ACTION helptext2 {
	PLAY_SOUND menu_snd,1;
	SET IF_CUR,helptext3;
	SET IF_CUL,helptext1;
	SET help_txt.STRING,help_str2;}

ACTION helptext3 {
	PLAY_SOUND menu_snd,1;
	SET IF_CUR,helptext4;
	SET IF_CUL,helptext2;
	SET help_txt.STRING,help_str3;}

ACTION helptext1 {
	PLAY_SOUND menu_snd,1;
	SET IF_CUL,NULL;
	SET IF_CUR,helptext2;
	SET help_txt.STRING,help_str;}

//////////////////////////////
ACTION show_spells {
	PLAY_SOUND menu_snd,1;
	IF_NEQUAL LAYERS.1,spellbook_ovr;
		SKIP 3;
	SET spell_txt.VISIBLE,0;
	SET LAYERS.1,NULL;
	END;
	IF_EQUAL LAYERS.3,inventory_ovr;
		CALL show_inventory;
	IF_EQUAL LAYERS.2,character_ovr;
		CALL show_character;
	SET spell_txt.VISIBLE,1;
	SET LAYERS.1,spellbook_ovr;}

ACTION show_character {
	PLAY_SOUND menu_snd,1;
	IF_NEQUAL LAYERS.2,character_ovr;
		SKIP 4;
	SET PANELS.2,NULL;
	SET LAYERS.2,NULL;
	SET char_txt.VISIBLE,0;
	END;
	IF_EQUAL LAYERS.3,inventory_ovr;
		CALL show_inventory;
	IF_EQUAL LAYERS.1,spellbook_ovr;
		CALL show_spells;
	SET char_txt.VISIBLE,1;
	SET PANELS.2,char_pan;
	SET LAYERS.2,character_ovr;}

//////////Save & Load//////////
ACTION show_load {
SET svld_txt.STRING,ld_str;
CALL reset_controls;
SET IF_ENTER,choose_filename;
SET IF_RIGHT,choose_filename;
SET IF_F3,show_slots;	
CALL show_slots;
}

ACTION show_save {
IF_BELOW levelnr,90; SKIP 3;
SET newstring,1;
SET info_str, nosave_str;
END;
SET svld_txt.STRING,sv_str;
CALL reset_controls;
SET IF_ENTER,edit_filename;
SET IF_RIGHT,edit_filename;
SET IF_F2,show_slots;
CALL show_slots;
}

ACTION show_slots {
	PLAY_SOUND mselect_snd,1;
	IF_NEQUAL LAYERS.13,slots_ovr;
		GOTO show_slot;
	SET svld_txt.VISIBLE,NULL;
	SET LAYERS.13,NULL;
	SET slot1_txt.VISIBLE,0;
	SET slot2_txt.VISIBLE,0;
	SET slot3_txt.VISIBLE,0;
	SET slot4_txt.VISIBLE,0;
	SET slot5_txt.VISIBLE,0;
	SET slot6_txt.VISIBLE,0;
	SET option,1;
	SET menuborder_ovr.VISIBLE,0;
	SET menuborder_ovr.POS_Y,88;
	SET IF_ENTER,NULL;
	IF_EQUAL load_me,1;
		END;
	IF_EQUAL PANELS.10,title_pan;
		SKIP 4;
	CALL set_controls;
	SET MOVE_MODE,1;
	SET EACH_TICK.16,move_me;
	END;
	CALL show_menu;
	END;
	
show_slot:
	SET option,1;
	SET menuborder_ovr.POS_Y,88;
	SET LAYERS.13,slots_ovr;
	SET svld_txt.VISIBLE,1;
	SET MOVE_MODE,0;
	SET EACH_TICK.16,NULL;
	SET mapborder_ovr.VISIBLE,0;
	SET MAP_MODE,0;
	SET slot1_txt.VISIBLE,1;
	SET slot2_txt.VISIBLE,1;
	SET slot3_txt.VISIBLE,1;
	SET slot4_txt.VISIBLE,1;
	SET slot5_txt.VISIBLE,1;
	SET slot6_txt.VISIBLE,1;
	SET menuborder_ovr.VISIBLE,1;
	SET IF_CUU,choose_opt_up;
	SET IF_CUD,choose_opt_dn;
	SET IF_MIDDLE,choose_opt_up;
	SET IF_LEFT,choose_opt_dn;
	SET IF_ESC,show_slots;
}


ACTION choose_filename {
	SET slot,option;
	CALL load_game;
}

ACTION edit_filename {
	SET slot,option;
	IF_NEQUAL slot,1;
		SKIP 3;
	INKEY slot1_str;
	IF_EQUAL result,0;
		CALL save_game;
	IF_NEQUAL slot,2;
		SKIP 3;
	INKEY slot2_str;
	IF_EQUAL result,0;
		CALL save_game;
	IF_NEQUAL slot,3;
		SKIP 3;
	INKEY slot3_str;
	IF_EQUAL result,0;
		CALL save_game;
	IF_NEQUAL slot,4;
		SKIP 3;
	INKEY slot4_str;
	IF_EQUAL result,0;
		CALL save_game;
	IF_NEQUAL slot,5;
		SKIP 3;
	INKEY slot5_str;
	IF_EQUAL result,0;
		CALL save_game;
	IF_NEQUAL slot,6;
		SKIP 3;
	INKEY slot6_str;
	IF_EQUAL result,0;
		CALL save_game;
}
//////////Main Menu//////////
ACTION show_menu {
PLAY_SOUND mselect_snd,1;
IF_NEQUAL menu_txt.VISIBLE,1;GOTO open_menu;
SET menu_txt.VISIBLE,NULL;
SET LAYERS.16,NULL;
SET option,1;
SET menuborder_ovr.VISIBLE,0;
SET menuborder_ovr.POS_Y,88;
CALL darkening_off;
IF_NEQUAL PANELS.10,title_pan; 
	SKIP 7;
SET IF_ENTER,NULL;
SET IF_CUU,NULL;        
SET IF_CUD,NULL;
SET IF_RIGHT,NULL;
SET IF_LEFT,NULL;        
SET IF_MIDDLE,NULL;
END;
CALL set_controls;
SET MOVE_MODE,1;
END;

open_menu:
SET EACH_TICK.5,NULL; //Intro
SET option,1;
SET menuborder_ovr.POS_Y,88;
IF_NEQUAL PANELS.10,title_pan;SET LAYERS.16,menu_ovr;
SET menu_txt.VISIBLE,1;
CALL darkening_on;
SET MOVE_MODE,-0.5;
IF_EQUAL PANELS.10,NULL; SKIP 3;
SET MOVE_MODE,0;
SET EACH_TICK.2,NULL;
SET MESSAGES.1,NULL;

SET mapborder_ovr.VISIBLE,0;
SET MAP_MODE,0;
CALL reset_controls;
SET menuborder_ovr.VISIBLE,1;
SET IF_ENTER,check_selection;
SET IF_CUU,choose_opt_up;
SET IF_CUD,choose_opt_dn;
SET IF_RIGHT,check_selection;
SET IF_MIDDLE,choose_opt_up;
SET IF_LEFT,choose_opt_dn;
IF_NEQUAL PANELS.10,title_pan;
	SET IF_ESC,show_menu;
}

ACTION choose_opt_up {
RULE option=option-1;
RULE menuborder_ovr.POS_Y=menuborder_ovr.POS_Y-36;
PLAY_SOUND menu_snd,1;
IF_ABOVE option,0;
	END;
SET option,6;
SET menuborder_ovr.POS_Y,268;}

ACTION choose_opt_dn {
RULE option=option+1;
RULE menuborder_ovr.POS_Y=menuborder_ovr.POS_Y+36;
PLAY_SOUND menu_snd,1;
IF_BELOW option,7;
	END;
SET option,1;
SET menuborder_ovr.POS_Y,88;}


ACTION check_selection {
        IF_NEQUAL option,1;
                SKIP 2;
        CALL show_menu;
        BRANCH new_game;
        IF_NEQUAL option,2;
                SKIP 4;
        IF_EQUAL PANELS.10,title_pan;
		BRANCH restore_intro; //test!!! #END;
	CALL show_menu;
        BRANCH show_save;
        IF_NEQUAL option,3;
                SKIP 2;
        CALL show_menu;
        BRANCH show_load;
	IF_NEQUAL option,4;
		SKIP 2;
	CALL show_menu;
	BRANCH show_options;
	IF_NEQUAL option,5;
		SKIP 2;
	CALL show_menu;
	BRANCH show_help;
	IF_NEQUAL option,6;
		SKIP 2;
	CALL show_menu;
	BRANCH quit_game;
	END;}

//////////Options//////////

ACTION show_options {
	PLAY_SOUND mselect_snd,1;
	IF_NEQUAL LAYERS.11,options_ovr;
		GOTO show_option;
	SET LAYERS.11,NULL;
	SET options_txt.VISIBLE,NULL;
	SET opt_switch_ovr1.VISIBLE,0;
	SET opt_switch_ovr2.VISIBLE,0;
	SET opt_switch_ovr3.VISIBLE,0;
	SET MESSAGES.11,NULL;
	SET hi_lo_txt.STRING,nix_str;
	SET option,1;
	SET menuborder_ovr.VISIBLE,0;
	SET menuborder_ovr.POS_Y,88;
	CALL darkening_off;
	SET IF_ESC,show_menu;
        IF_EQUAL PANELS.10,title_pan;
        	SKIP 3;
        CALL set_controls;
        SET MOVE_MODE,1;
	END;
	SET IF_ESC,NULL;
	SAVE_INFO "WoK0",0;
	CALL show_menu;
	END;
	
show_option:
	SET LAYERS.11,options_ovr;
	SET MESSAGES.11,hi_lo_txt;
	SET options_txt.VISIBLE,1;
        SET opt_switch_ovr1.VISIBLE,1;
	SET opt_switch_ovr2.VISIBLE,1;
	SET opt_switch_ovr3.VISIBLE,1;
        CALL darkening_on;
	SET MOVE_MODE,-0.5;
IF_NEQUAL PANELS.10,NULL; SET MOVE_MODE,0;
	SET mapborder_ovr.VISIBLE,0;
	SET MAP_MODE,0;
	CALL reset_controls;
	CALL set_motionblur;
	CALL set_switches;
	SET menuborder_ovr.POS_Y,16;
	SET menuborder_ovr.VISIBLE,1;
	SET IF_CUL,check_sel_left;
	SET IF_CUR,check_sel_right;
	SET IF_CUU,choose_opt_up2;
	SET IF_CUD,choose_opt_dn2;
	SET IF_MIDDLE,choose_opt_up2;
	SET IF_LEFT,choose_opt_dn2;
	SET IF_F12,show_options;
	SET IF_ESC,show_options;
}

ACTION choose_opt_up2 {
	RULE option=option-1;
	RULE menuborder_ovr.POS_Y=menuborder_ovr.POS_Y-72;
	PLAY_SOUND menu_snd,1;
	IF_ABOVE option,0;
		END;
	SET option,4;
	SET menuborder_ovr.POS_Y,232;}

ACTION choose_opt_dn2 {
	RULE option=option+1;
	RULE menuborder_ovr.POS_Y=menuborder_ovr.POS_Y+72;
	PLAY_SOUND menu_snd,1;
	IF_BELOW option,5;
		END;
	SET option,1;
	SET menuborder_ovr.POS_Y,16;}

ACTION check_sel_left {
        IF_NEQUAL options_txt.VISIBLE,1;     
                END;
	PLAY_SOUND menu_snd,1;
	IF_NEQUAL option,1;
		SKIP 2;
	SET MOTION_BLUR,0.5;
	CALL set_motionblur;
	IF_NEQUAL option,2;
		SKIP 2;
	ADD SOUND_VOL,-0.1;
	CALL set_switches;
	IF_NEQUAL option,3;
		SKIP 2;
	ADD MUSIC_VOL,-0.1;
	CALL set_switches;
	IF_NEQUAL option,4;
		END;
	ADD gamma,-0.04;
	IF_EQUAL underwater,0;
		FADE_PAL nebel_pal,gamma;
	CALL set_switches;
}

ACTION check_sel_right {
        IF_NEQUAL options_txt.VISIBLE,1;     
                END;
	PLAY_SOUND menu_snd,1;
	IF_NEQUAL option,1;
		SKIP 2;
	SET MOTION_BLUR,0;
	CALL set_motionblur;
	IF_NEQUAL option,2;
		SKIP 2;
	ADD SOUND_VOL,0.1;
	CALL set_switches;
	IF_NEQUAL option,3;
		SKIP 2;
	ADD MUSIC_VOL,0.1;
	CALL set_switches;
	IF_NEQUAL option,4;
		END;
	ADD gamma,0.04;
	IF_EQUAL underwater,0;
		FADE_PAL nebel_pal,gamma;
	CALL set_switches;
}

ACTION set_switches {
	RULE switch1=(SOUND_VOL*70)+121;
	RULE switch2=(MUSIC_VOL*70)+121;
	RULE switch3=(gamma*350)+120;
	SET opt_switch_ovr1.POS_X,switch1;
	SET opt_switch_ovr2.POS_X,switch2;
	SET opt_switch_ovr3.POS_X,switch3;
	}

ACTION set_motionblur {
	IF_EQUAL MOTION_BLUR,0;
		SET resolution_str,hires_str;
	IF_EQUAL MOTION_BLUR,0.5;
		SET resolution_str,lores_str;
	SET hi_lo_txt.STRING,resolution_str;}

//////////Training//////////

ACTION show_training {
	SET MAP_MODE,0;
	SET mapborder_ovr.VISIBLE,0;
	IF_NEQUAL LAYERS.2,character_ovr;
		CALL show_character;
	SET LAYERS.1,NULL;
	SET spell_txt.VISIBLE,0;
	SET training_txt.VISIBLE,1;
	CALL reset_controls;
	SET IF_ENTER, add_point;
	SET IF_CUU, choose_skill_up;
	SET IF_CUD, choose_skill_dn;
	SET marker_txt.POS_Y,156;
	SET marker_txt.VISIBLE,1;
	SET option,1;
	SET PANELS.3,pts_left_pan;
	SET char_txt.INDEX,2;
	SET char_txt.STRING,pts_left_str;
	CALL stop_moving;
	SET EACH_TICK.16,NULL;
}

ACTION choose_skill_up {
	RULE option=option-1;
	RULE marker_txt.POS_Y=marker_txt.POS_Y-18;
	IF_ABOVE option,0;
		END;
	SET option,4;
	SET marker_txt.POS_Y,210;}

ACTION choose_skill_dn {
	RULE option=option+1;
	RULE marker_txt.POS_Y=marker_txt.POS_Y+18;
	IF_BELOW option,5;
		END;
	SET option,1;
	SET marker_txt.POS_Y,156;}

ACTION add_point {
	IF_EQUAL option,1;
		ADD str,1;
	IF_EQUAL option,2;
		ADD con,1;
	IF_EQUAL option,3;
		ADD int,1;
	IF_EQUAL option,4;
		ADD spd,1;
	SUB pts_left,1;
	CALL check_skills;
	IF_ABOVE pts_left,0;
		END;
	SET PANELS.3,NULL;
	SET char_txt.INDEX,2;
	SET char_txt.STRING,nix_str;
	SET MOVE_MODE,1;
	SET char_txt.INDEX,2;
	SET char_txt.STRING,nix_str;
	SET marker_txt.VISIBLE,NULL;
	SET training_txt.VISIBLE,NULL;
	SET PANELS.3,NULL;
	SET option,1;
	CALL show_character;	
	SET IF_ENTER,NULL;
	SET IF_CUU,NULL;
	SET IF_CUD,NULL;
	CALL set_controls;
	SET EACH_TICK.16,move_me;
}		
	
ACTION check_skills {
	RULE weight.MAX=str*9+0.05+70;
	RULE dmg=str/5;
	RULE armor=spd/5+AC;
}

ACTION set_gamma {
	IF_BELOW gamma,0.18;
		SKIP 2;
	SET gamma,0.00;
	SKIP 1;
	ADD gamma,0.04;
	IF_EQUAL underwater,0;
		FADE_PAL nebel_pal,gamma;
	CALL set_switches;
}

//////////New Game//////////

ACTION new_game {
IF_NEQUAL PANELS.10,NULL;
BRANCH new;
SET mapborder_ovr.VISIBLE,0;
SET MOVE_MODE,-0.5;
SET MAP_MODE,0;
CALL darkening_on;
SET MESSAGES.16,new_txt;
CALL reset_controls;
SET IF_ESC,no_new;SET IF_N,no_new;SET IF_Z,new;SET IF_Y,new;SET IF_J,new;}

ACTION no_new {
SET MOVE_MODE,1;
SET MESSAGES.16,NULL;
CALL darkening_off;
SET IF_ESC,NULL;SET IF_N,NULL;SET IF_Z,NULL;SET IF_Y,NULL;SET IF_J,NULL;
CALL set_controls;}

ACTION new {
SET EACH_TICK.2,NULL;
SET EACH_TICK.8,NULL;
SET BLUR_MODE,1;
SET info_txt.STRING,wait_str;
SET info_txt.VISIBLE,1;
SAVE_INFO "WoK0",0;
SET MOVE_MODE,1;
CALL set_walking;
CALL darkening_off;
SET IF_ESC,NULL;
SET IF_N,NULL;SET IF_Z,NULL;SET IF_Y,NULL;SET IF_J,NULL;
CALL set_controls;
SET messages.16,NULL;
SET title_pan.POS_Y,380;
SET LAYERS.14,display2_ovr;
SET PANELS.13,display_pan;
SET disp_txt.VISIBLE,1;
SET SCREEN_HGT,326;
WAIT 1;
IF_NEQUAL PANELS.10,title_pan;
BRANCH new2;
#SET levelnr,90;
SET PANELS.10,NULL;
SET PANELS.11,NULL;
SET MESSAGES.1,NULL;
#SET info_txt.VISIBLE,NULL;
#	SAVE "WoK0",9;
}

ACTION new2{LOAD "WoK0",9;}

ACTION reload_game {
        SET mapborder_ovr.VISIBLE,0;
	SET MAP_MODE,0;
	SET MOVE_MODE,0.5;
	CALL darkening_on;
	SET MESSAGES.16,reload_txt;
	CALL reset_controls;
	SET IF_ESC,new;SET IF_N,new;SET IF_Z,prep_reload;SET IF_Y,prep_reload;SET IF_J,prep_reload;}

ACTION prep_reload {
	SET messages.16,NULL;
        SET IF_ESC,NULL;SET IF_N,NULL;SET IF_Z,NULL;SET IF_Y,NULL;SET IF_J,NULL;
	BRANCH show_load;
}
//////////Quit Game//////////
ACTION quit_game {
	SET mapborder_ovr.VISIBLE,0;
	SET MAP_MODE,0;
	SET MOVE_MODE,-0.5;
	CALL darkening_on;
	SET quit_txt.VISIBLE,1;
	CALL reset_controls;
	SET IF_ESC,no_quit;
	SET IF_N,no_quit;
	SET IF_Z,quit;
	SET IF_Y,quit;
	SET IF_J,quit;}

ACTION no_quit {
	SET quit_txt.VISIBLE,0;
	CALL darkening_off;
	SET IF_ESC,NULL;
	SET IF_N,NULL;
	SET IF_Z,NULL;
	SET IF_Y,NULL;
	SET IF_J,NULL;
        IF_NEQUAL PANELS.10,title_pan; 
                SKIP 2;        
        CALL show_menu;
        END;
        CALL set_controls;
        SET MOVE_MODE,1;
}

ACTION quit {
SAVE_INFO "WoK0",0;
EXIT "             Thanx for playing the Demo of The World of Kandoria!!!
                       This game was created by Firoball

                                 Firoball is:
                        Robert J„ger
                        eMail: master@firoball.de
                        WWW: http://www.firoball.de
                        ICQ: 12525151 or look for Firoball

\n";
}
//////////Tastaturbelegung//////////

ACTION reset_controls {
SET IF_BKSP,NULL; SET IF_0,NULL; SET IF_1, NULL;SET IF_2, NULL;
SET IF_3, NULL;SET IF_4, NULL;SET IF_5, NULL;SET IF_6, NULL;
SET IF_INS,NULL; SET IF_DEL,NULL; SET IF_HOME,NULL;
SET IF_CTRL,NULL; SET IF_LEFT,NULL;SET IF_ANYKEY,NULL;
SET IF_ESC,NULL;SET IF_H,NULL; SET IF_TAB,NULL;
SET IF_N,NULL; SET IF_F1,NULL;SET IF_B,NULL; SET IF_F2,NULL;
SET IF_F,NULL; SET IF_F3,NULL;SET IF_L,NULL; SET IF_F4,NULL;
SET IF_E,NULL; SET IF_F5,mach_nix;SET IF_M,NULL; SET IF_F6,NULL;
SET IF_R,NULL; SET IF_F7,NULL;SET IF_P,NULL; SET IF_F8,NULL;
SET IF_SPACE,NULL; SET IF_F9,NULL;SET IF_RIGHT,NULL; SET IF_F12,mach_nix;
SET IF_F11,NULL;SET IF_e,NULL; SET IF_r,NULL;SET IF_D,NULL;
SET IF_t,NULL; SET IF_y,NULL;SET IF_F10,mach_nix;}

ACTION set_controls {
SET IF_BKSP,unequip_ainv; SET IF_0, unequip_qinv; SET IF_1, hotkey_use;SET IF_2, hotkey_use;
SET IF_3, hotkey_use;SET IF_4, hotkey_use;SET IF_5, hotkey_use;SET IF_6, hotkey_use;

IF_NEQUAL LAYERS.3,inventory_ovr; SKIP 3;
SET IF_INS,inv_up; SET IF_DEL,inv_down; SET IF_HOME,inv_use;
	
SET IF_D,drop_item;SET IF_CTRL,attack; SET IF_LEFT,attack; SET IF_MIDDLE,NULL;
SET IF_ENTER,use_invitem;SET IF_CUU,NULL;SET IF_CUD,NULL;
SET IF_CUL,NULL;SET IF_CUR,NULL;SET IF_TAB,show_map;
SET IF_P,pause_game;SET IF_SPACE,use_item; SET IF_RIGHT,use_item;
SET IF_RIGHT,use_item;SET IF_ESC,show_menu;SET IF_F1,show_help;
SET IF_F2,show_save;SET IF_F3,show_load;SET IF_F4,new_game;
SET IF_F5,mach_nix;SET IF_F6,show_inventory;SET IF_F7,show_character;
SET IF_F8,show_spells;
	SET IF_E,change_level1;
	SET IF_R,change_level2;
	SET IF_T,change_level3;
	SET IF_y,change_level4;
SET IF_H,heal_spell;SET IF_N,night_spell;
#	SET IF_F,fly_spell;
SET IF_F11,set_gamma;SET IF_F12,show_options;SET IF_F10,quit_game;
SET IF_APO,mach_nix; SET IF_SZ,mach_nix;
}

ACTION attack {
IF_NEQUAL EACH_TICK.3,NULL;
	END;
IF (levelnr==90) {SET newstring,1; SET info_str,noweapon_str;END;}
SET EACH_TICK.3,weapon_atk;}

ACTION mach_nix {SET MAP_MODE,MAP_MODE;}

ACTION darkening_on {
	SET player_light_s,PLAYER_LIGHT;
	SET PLAYER_LIGHT,0.2;
	SET RENDER_MODE,1;}

ACTION darkening_off {
	SET PLAYER_LIGHT,player_light_s;
	SET RENDER_MODE,1;}

//////////Kameras//////////
//Kameras NICHT in Saeure oder Lava aufstellen, sonst gibt's Fehler!!!

ACTOR player_old {
	TEXTURE black_tex;
	FLAGS INVISIBLE,CAREFULLY,SAVE;}

THING camera1 {
	TEXTURE black_tex;
	HEIGHT 15;
	SKILL8 0.5;
	FLAGS INVISIBLE,SAVE;}

THING camera2 {
	TEXTURE black_tex;
	HEIGHT 2;
	SKILL8 0.2;
	FLAGS INVISIBLE,SAVE;}

THING camera3 {
	TEXTURE black_tex;
	HEIGHT 9;
	SKILL8 0.2;
	FLAGS INVISIBLE,SAVE;}

THING camera4 {
	TEXTURE black_tex;
	HEIGHT 422;
	SKILL8 0.3;
	FLAGS INVISIBLE,SAVE;}

THING camera5 {
	TEXTURE black_tex;
	HEIGHT 9;
	SKILL8 0.1;
	FLAGS INVISIBLE,SAVE;}

ACTION show_camera {
	SET camera.FLAG7,1;
	WAIT 1;
	CALL cutscene_on;
	CALL set_camera;
	SET PLAYER_X,camera.X;
	SET PLAYER_Y,camera.Y;
	SET PLAYER_ANGLE,camera.ANGLE;
	SET PLAYER_TILT,camera.SKILL8;
	SET HERE,camera.REGION;
	RULE PLAYER_Z=HERE.FLOOR_HGT+camera.HEIGHT;
	SET LAYERS.4,NULL;
	CALL reset_controls;
#	SET camera_txt.VISIBLE,1;
	WAIT 1;
	}

ACTION show_camera1 {SET camera,camera1;
	CALL show_camera;}

ACTION show_camera2 {SET camera,camera2;
	WAIT 1;
	CALL show_camera;}

ACTION show_camera3 {SET camera,camera3;
	FADE_PAL pal_blue,0.5;
	CALL show_camera;}

ACTION show_camera4 {SET camera,camera4;
	CALL show_camera;}

ACTION show_camera5 {SET camera,camera5;
	CALL show_camera;}

ACTION stop_camera {
	IF_EQUAL camera.FLAG7,0;
		END;
	SET camera.FLAG7,0;
	BRANCH set_player;
	}

ACTION stop_camera1 {
	IF_EQUAL camera.FLAG7,0;
		END;
	SET camera.FLAG7,0;
	BRANCH set_player;
	}

ACTION stop_camera2 {
	IF_EQUAL camera.FLAG7,0;
		END;
	SET camera.FLAG7,0;
	BRANCH set_player;
	}

ACTION stop_camera4 {
	IF_EQUAL camera.FLAG7,0;
		END;
	SET camera.FLAG7,0;
	BRANCH set_player;
	}

ACTION stop_camera3 {
	IF_EQUAL camera3.FLAG7,0;
		END;
	SET camera3.FLAG7,0;
	FADE_PAL pal_blue,0;
        FADE_PAL nebel_pal,gamma;
        BRANCH set_player;
	}

ACTION set_camera {        
	IF_EQUAL night,1;
		SET PANELS.14,NULL;
	SET EACH_TICK.16,NULL;
	SET MAP_MODE,0;
	SET mapborder_ovr.VISIBLE,0;
	SET PANELS.2,NULL;
	SET LAYERS.1,NULL;
	SET LAYERS.2,NULL;
	SET char_txt.VISIBLE,0;
	SET spell_txt.VISIBLE,0;
	SET MESSAGES.10,NULL;
	SET player_old.REGION,HERE;
	SET player_old.X,PLAYER_X;
	SET player_old.Y,PLAYER_Y;
	SET player_old.Z,PLAYER_Z;
	SET player_old.ANGLE,PLAYER_ANGLE;
	SET player_old.SKILL4,PLAYER_HGT;
	SET player_old.SKILL5,PLAYER_TILT;
	CALL stop_moving;}

ACTION set_player {        
	CALL cutscene_off;
	SET camera_txt.VISIBLE,0;
	IF_EQUAL night,1;
		SET PANELS.14,night_pan;
	SET MESSAGES.10,anzeige_txt;
	SET PLAYER_X,player_old.X;
	SET PLAYER_Y,player_old.Y;
	SET PLAYER_ANGLE,player_old.ANGLE;
	SET PLAYER_TILT,player_old.SKILL5;
	SET IF_ESC,NULL;
	SET HERE,player_old.REGION;
	SET PLAYER_Z,player_old.z;
	SET LAYERS.4,weapon_ovr;
	CALL set_moving;
	CALL set_controls;
}

ACTION check_spells {
#	SET spell_txt.INDEX,2;
#	SET spell_txt.STRING,spell1_str;
#	IF_EQUAL heal,0;
#		SET spell_txt.STRING,nix_str;
	SET spell_txt.INDEX,3;
	SET spell_txt.STRING,spell3_str;
	IF_EQUAL nightvision,0;
		SET spell_txt.STRING,nix_str;
	SET spell_txt.INDEX,4;
	SET spell_txt.STRING,spell3_str;
	IF_EQUAL magic_arrow,0;
		SET spell_txt.STRING,nix_str;
	SET spell_txt.INDEX,5;
	SET spell_txt.STRING,spell4_str;
	IF_EQUAL fireball,0;
		SET spell_txt.STRING,nix_str;
	SET spell_txt.INDEX,6;
	SET spell_txt.STRING,spell5_str;
	IF_EQUAL fly,0;
		SET spell_txt.STRING,nix_str;
	SET spell_txt.INDEX,7;
	SET spell_txt.STRING,spell6_str;
	IF_EQUAL shield,0;
		SET spell_txt.STRING,nix_str;
	SET spell_txt.INDEX,8;
	SET spell_txt.STRING,spell7_str;
	IF_EQUAL lightning,0;
		SET spell_txt.STRING,nix_str;
	SET spell_txt.INDEX,9;
	SET spell_txt.STRING,spell8_str;
	IF_EQUAL shield,0;
		SET spell_txt.STRING,nix_str;
	SET spell_txt.INDEX,10;
	SET spell_txt.STRING,spell9_str;
	IF_EQUAL explosion,0;
		SET spell_txt.STRING,nix_str;
	SET spell_txt.INDEX,11;
	SET spell_txt.STRING,spell10_str;
	IF_EQUAL town_portal,0;
		SET spell_txt.STRING,nix_str;
	}
//////////Zaubersprueche//////////
STRING fly_str,"i cannot use this spell underwater";
ACTION fly_spell {
	IF_EQUAL moving,Mode_Fliegen;
		BRANCH set_walking;
	IF_EQUAL fly,1;
		SKIP 3;
	SET newstring,1;
	SET info_str,nospell_str;
	END;
	IF_ABOVE MP,4.49;
		SKIP 3;
	SET newstring,1;
	SET info_str,nomana_str;
	END;
	IF_EQUAL underwater,0;
		BRANCH set_flying;
	SET newstring,1;
	SET info_str,fly_str;
}
SOUND heal_snd, <heal.wav>;
STRING heal_str,"LIFE IS ALREADY AT MAXIMUM";
ACTION heal_spell {
	RULE heal=HP.MAX-0.5;
	IF_BELOW HP,heal;
		SKIP 3;
	SET newstring,1;
	SET info_str,heal_str;
	END;
	IF_ABOVE MP,4.49;
		SKIP 3;
	SET newstring,1;
	SET info_str,nomana_str;
	END;
	PLAY_SOUND heal_snd,0.9;
	RULE heal=RANDOM*10;
	ADD HP,heal;
	SUB MP,5;
}

SOUND nvision_snd, <nvision.wav>;
SOUND nvision2_snd, <nvision2.wav>;
STRING night_str,"NIGHT VISION ACTIVATED";
STRING night_str2,"NIGHT VISION DEACTIVATED";
ACTION night_spell {
	IF_EQUAL night,1;
		SKIP 19;
	IF_EQUAL Nightvision,1;
		SKIP 3;
	SET newstring,1;
	SET info_str,nospell_str;
	END;
	IF_ABOVE MP,4.49;
		SKIP 3;
	SET newstring,1;
	SET info_str,nomana_str;
	END;
	SET night,1;
	SET newstring,1;
	SET info_str,night_str;
	SET normlight,AMBIENT;
	PLAY_SOUND nvision_snd,0.9;
	SET AMBIENT,0.45;
	SET RENDER_MODE,1;
	SET PANELS.14,night_pan;
	END;
	SET newstring,1;
	SET info_str,night_str2;
	PLAY_SOUND nvision2_snd,0.9;
	SET night,0;
	SET AMBIENT,normlight;
	SET RENDER_MODE,1;
	SET PANELS.14,NULL;
}

//////////Flackerregionen//////////
ACTION flicker_cave {
	SET_ALL THERE.AMBIENT,-0.1;
	RULE THERE.SKILL8=RANDOM*8;
	WAITT THERE.SKILL8;
	RULE THERE.SKILL7=RANDOM/4;
	SET_ALL THERE.AMBIENT,THERE.SKILL7;}

ACTION flicker_cave_d {
	SET_ALL THERE.AMBIENT,-0.2;
	RULE THERE.SKILL8=RANDOM*8;
	WAITT THERE.SKILL8;
	RULE THERE.SKILL7=(RANDOM-1)/4;
	SET_ALL THERE.AMBIENT,THERE.SKILL7;}
