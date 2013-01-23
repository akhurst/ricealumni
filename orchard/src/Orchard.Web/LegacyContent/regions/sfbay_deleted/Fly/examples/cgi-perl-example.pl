#!/usr/local/bin/perl

print "Content-type: image/gif\n\n";

$flyprog = "/usr/local/web/bin/fly";

$infile = "/tmp/fly.$$";

open(FLY,"> $infile");
print FLY "new\n";
print FLY "size 256,256\n";
print FLY "fill 1,1,255,255,255\n";
print FLY "arc 128,128,180,180,0,360,0,0,0\n";
print FLY "fill 128,128,255,255,0\n";
print FLY "arc 128,128,120,120,0,180,0,0,0\n";
print FLY "arc 96,96,10,10,0,360,0,0,0\n";
print FLY "arc 160,96,10,10,0,360,0,0,0\n";
print FLY "fill 96,96,0,0,0\n";
print FLY "fill 160,96,0,0,0\n";
print FLY "string 0,0,0,10,240,giant,Hello, World!\n";
print FLY "string 0,0,0,100,10,medium,Don't worry, be Happy!\n";

close(FLY);

open(FOO,"$flyprog -q -i $infile |");
while( <FOO> ) {print;}
close(FOO);

unlink($infile);

exit(0);
