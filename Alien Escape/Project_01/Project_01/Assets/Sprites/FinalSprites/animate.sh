#!/bin/sh
# this submission
origin=scifi_hackspace_frama.png
# from https://opengameart.org/content/lpc-goes-to-space
scifi_space=scifi_space_rpg_tiles_lpcized.png

## icons
magick ${scifi_space} -crop 32x$((3*32))+$((3*32))+0 +repage icons_floor.png
magick ${origin} -crop 32x$((3*32))+$((10*32))+0 +repage icons_on.png
magick ${origin} -crop 32x$((3*32))+$((10*32))+$((3*32)) +repage icons_off.gif
magick icons_floor.png icons_on.png -flatten icons_on.gif
gifsicle --delay=60 --loop icons_on.gif icons_off.gif > icons.gif

## led bulbs
# build wall
magick ${scifi_space} -crop 32x64+$((7*32))+32 +repage led_wall_a.png
magick ${scifi_space} -crop 32x64+$((10*32))+32 +repage led_wall_b.png
magick montage -geometry +0+0 -tile 6x1 led_wall_b.png led_wall_a.png led_wall_b.png led_wall_b.png led_wall_b.png led_wall_a.png led_wall.png
magick ${origin} -crop 64x32+$((8*32))+$((7*32)) +repage led_cable.png
magick led_wall.png led_cable.png -geometry +32+0 -composite led_cable.png -geometry +64+0 -composite led_cable.png -geometry +96+0 -composite led_cable.png -geometry +128+0 -composite led_cable.png -flatten led_walled.png
# build led frames
magick ${origin} -crop 32x32+$((8*32))+$((2*32)) +repage led_off.png
magick ${origin} -crop 32x32+$((9*32))+$((2*32)) +repage led_red.png
magick ${origin} -crop 32x32+$((8*32))+$((3*32)) +repage led_green.png
magick ${origin} -crop 32x32+$((9*32))+$((3*32)) +repage led_blue.png
magick montage -geometry +0+0 -tile 6x1 led_off.png led_blue.png led_off.png led_green.png led_off.png led_red.png -background black leds_0.png
magick montage -geometry +0+0 -tile 6x1 led_blue.png led_off.png led_green.png led_off.png led_red.png led_off.png -background black leds_1.png
magick montage -geometry +0+0 -tile 6x1 led_off.png led_green.png led_off.png led_red.png led_off.png led_blue.png -background black leds_2.png
magick montage -geometry +0+0 -tile 6x1 led_green.png led_off.png led_red.png led_off.png led_blue.png led_off.png -background black leds_3.png
magick montage -geometry +0+0 -tile 6x1 led_off.png led_red.png led_off.png led_blue.png led_off.png led_green.png -background black leds_4.png
magick montage -geometry +0+0 -tile 6x1 led_red.png led_off.png led_blue.png led_off.png led_green.png led_off.png -background black leds_5.png
for i in {0..5}; do
    magick led_walled.png \( leds_${i}.png -transparent black \) -flatten leds_${i}.gif
done
gifsicle --delay=30 --loop leds_?.gif > leds.gif

## led wall
magick ${origin} -crop 64x64+$((4*32))+$((4*32)) +repage ledwall_rgb.png
magick ${origin} -crop 64x64+$((6*32))+$((4*32)) +repage ledwall_gb.png
magick ${origin} -crop 64x64+$((4*32))+$((6*32)) +repage ledwall_rb.png
magick ${origin} -crop 64x64+$((6*32))+$((6*32)) +repage ledwall_rg.png
magick ${origin} -crop 64x64+$((8*32))+$((4*32)) +repage ledwall_off.png
magick ${origin} -crop 64x32+$((8*32))+$((6*32)) +repage ledwall_border.png
magick ${origin} -crop 64x32+$((2*32))+$((7*32)) +repage ledwall_text.png
for i in rgb gb rb rg off; do
    magick montage -geometry +0+0 -tile 1x2 ledwall_${i}.png ledwall_border.png -background black ledwall_${i}.png
    magick \( ledwall_${i}.png -transparent black \) ledwall_text.png -geometry +0+32 -composite ledwall_${i}.gif
done
gifsicle --loop ledwall_rgb.gif ledwall_gb.gif ledwall_rb.gif ledwall_rg.gif ledwall_rgb.gif ledwall_off.gif | gifsicle -d40 "#0-3" -d60 "#4" -d20 "#5" > ledwall.gif

## obelisk
magick ${scifi_space} -crop 32x64+$((13*32))+$((3*32)) +repage obelisk_firstcrystal.png
magick ${origin} -crop $((6*32))x64+0+$((2*32)) +repage obelisk_crystal.png
magick montage -geometry +0+0 -tile 2x1 obelisk_firstcrystal.png obelisk_crystal.png -background black obelisk_crystal.png
magick ${origin} -crop 32x64+$((6*32))+$((2*32)) +repage obelisk_holder.png
# duplicate them
magick montage -geometry +0+0 -tile 1x2 obelisk_crystal.png obelisk_crystal.png -background black obelisk_crystal.png
magick montage -geometry +0+0 -tile 7x1 obelisk_holder.png obelisk_holder.png obelisk_holder.png obelisk_holder.png obelisk_holder.png obelisk_holder.png obelisk_holder.png -background black obelisk_holder.png
# layer them
magick obelisk_crystal.png \( obelisk_holder.png -transparent black \)  -geometry +0+32 -composite obelisk_combined.png
# split
magick obelisk_combined.png -crop 32x128 +repage -transparent black obelisk_%d.gif
gifsicle --loop obelisk_?.gif | gifsicle -d160 "#0" -d10 "#1-" > obelisk.gif

## flashing exit
magick ${origin} -crop 64x32+$((2*32))+$((4*32)) +repage exit_off.gif
magick ${origin} -crop 64x64+$((2*32))+$((5*32)) +repage exit_on.gif
gifsicle --loop exit_on.gif exit_off.gif | gifsicle -d60 "#0" -d10 "#1" > exit.gif


# remove temporary files
rm icons_floor.png icons_on.png icons_off.gif icons_on.gif
rm led_*.png leds_?.png leds_?.gif
rm ledwall_*.png ledwall_*.gif
rm obelisk_*.png obelisk_?.gif
rm exit_*.gif