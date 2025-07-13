

AS developer on Customer project for PPX Pos product you have been requested to change the welcome display message 
for 2 customers project.

PPX POS team send you new version of Pos.dll   . the PPX_Pos project should not be change and use only for referance and understadnig.

The message should display on Pos Load method , the Pos DLL was handed to you and should work as you can see in Main.cs file : 
by calling :   POS_Process.Load(IPos); where IPOS is POS API.

The message display by core Pos team is define as follows : "Hello Passport-X" you need to extend it so it will
show the message according to customer country :

for Examply for Italy it will be : "Hello Passport-X Italy customer".

You need to keep working with POS dll as it responsible to show the message in POS display device and
has more functionality that need to work .

In order to allow extension you can use the IPos API .
Your work should be on the CustomerPPx solution.

Good Luck!
