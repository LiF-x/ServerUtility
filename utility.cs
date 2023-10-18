/**
* <author>Christophe Roblin</author>
* <email>lifxmod@gmail.com</email>
* <url>https://github.com/LiF-x/ServerUtility</url>
* <credits>https://github.com/LiF-x/ServerUtility/graphs/contributors</credits>
* <description>Public repository to let everyone help on core functionality added to the LiFx serverautoloader</description>
* <license>GNU GENERAL PUBLIC LICENSE Version 3, 29 June 2007</license>
*/

package LiFxUtility
{
  /*
  * Loops the root Client list for specific pid and returns the GameConnection ScriptObject that represents that connection
  * @param int pid takes player identification
  * @return GameConnection client
  */
  function LiFxUtility::getPlayer(%pid)
  {
    for(%id = 0; %id < ClientGroup.getCount(); %id++)
    {
      %client = ClientGroup.getObject(%id);
      if(%pid == %client.getCharacterId())
      {
        return %client;
      }
    }
    return 0;
  }

  /*
  * Takes the 3 given vectors and returns a matrix with a null orientation added
  * @param int x x-vector
  * @param int y y-vector
  * @param int z z-vector
  * @return TransformF matrix
  */
  function LiFxUtility::createPositionTransform(%x, %y, %z)
  {
          %vec = %x SPC %y SPC %z;
          %nullorientation = "0 0 0 0";
          return MatrixCreate(%vec, %nullorientation);
  }

  /*
  * Take a shape name and search for it in the given group
  * @param string shapeName
  * @param list group
  * @return list<TSStatic> shapes
  */
  function LiFxUtility::findShapeFiles( %shape, %group) {
    %simSet = new SimSet(""){};
    foreach(%obj in %group){
      if(%obj.getClassName() $= "SimGroup") {
        LiFxRaidProtection.findShapeFiles(%shape,%obj);
      }
      else {
        if(%obj.getClassName() $= "SimSet") {
          LiFxRaidProtection.findShapeFiles(%shape,%obj);
        }
        else {
          if(%obj.getClassName() $= "TSStatic") {
            if(strpos(strlwr(%obj.shapeName), %shape) >= 0){
              %simSet.add(%obj);
            }
          }
        }
      }
    }
    return %simSet;
  }

  /*
  * Take position vector and transform it into GeoID
  * @param vector position Positional vector
  * @return int GeoID
  */
  function LiFxUtility::getGeoID(%position) {
    %z = nextToken(nextToken(%position, "x", " "), "y", " ");
    if (%x < -1024)
    {
        %x = mFloor((3064 + %x) * 0.25);
        if (%y < -1024)
        {
            
            %y = mFloor((3064 + %y) * 0.25);
            %t = 442;
        }
        else
        {
            if (%y < 1020)
            {
                %y = mFloor((1020 + %y) * 0.25);
                %t = 445;
            }
            else
            {
                %y = mFloor((%y - 1024) * 0.25);
                %t = 448;
            }
        }
    }
    else
    {
        if (%x < 1020)
        {
            %x = mFloor((1020 + %x) * 0.25);
            if (%y < -1024)
            {
                %y = mFloor((3064 + %y) * 0.25);
                %t = 443;
            }
            else
            {
                if (%y < 1020)
                {
                    %y = mFloor((1020 + %y) * 0.25);
                    %t = 446;
                }
                else
                {
                    %y = mFloor((%y - 1024) * 0.25);
                    %t = 449;
                }
            }
        }
        else
        {
            %x = mFloor((%x - 1024) * 0.25);
            if (%y < -1024)
            {
                %y = mFloor((3064 + %y) * 0.25);
                %t = 444;
            }
            else
            {
                if (%y < 1020)
                {
                    %y = mFloor((1020 + %y) * 0.25);
                    %t = 447;
                }
                else
                {
                    %y = mFloor((%y - 1024) * 0.25);
                    %t = 450;
                }
            }
        }
    }
    return ((%t << 18) | (%y << 9)) | %x;
  }
  
  /*
  * Loops the root Client list for specific pid and sends message to each client
  * @param channel int representing channel to send message over
  * @param message text message to send to the players
  */
  function LiFxUtility::messageAll(%channel, %message)
  {
    for(%id = 0; %id < ClientGroup.getCount(); %id++)
    {
      %client = ClientGroup.getObject(%id);
      %client.cmSendClientMessage(%channel, %message);
    }
  }
  function LiFxUtility::fromGeoID(%geoid) {
    if(isObject(LiFxCoordinates))
    {
      LiFxCoordinates.delete();
    }
    new ScriptObject (LiFxCoordinates) {
      geoid = $NULL;
      position = $NULL;
    };
    LiFxCoordinates.geoid = %geoid;
    LiFxCoordinates.position = LiFxUtility::getPositionFromGeoID(%geoid);
  }
  
  function LiFxUtility::fromPosition(%position) {
    if(isObject(LiFxCoordinates))
    {
      LiFxCoordinates.delete();
    }
    new ScriptObject (LiFxCoordinates) {
      geoid = $NULL;
      position = $NULL;
    };
    LiFxCoordinates.geoid = LiFxUtility::getGeoID(%position);
    LiFxCoordinates.position = %position;
  }
  function LiFxUtility::NorthEast(%this) {
    return VectorAdd(LiFxCoordinates.position, "1 0 0");
  }
  function LiFxUtility::NorthWest(%this) {
    return VectorAdd(LiFxCoordinates.position, "1 1 0");
  }
  function LiFxUtility::SouthEast(%this) {
    return LiFxCoordinates.position;
    
  }
  function LiFxUtility::SouthWest(%this) {
    return VectorAdd(LiFxCoordinates.position, "0 1 0");
  }
  function LiFxUtility::Center(%this) {
    return VectorAdd(LiFxCoordinates.position, "0.5 0.5 0");
    
  }

  function LiFxUtility::getGeoID(%position) {
    %z = nextToken(nextToken(%position, "x", " "), "y", " ");
    if (%x < -1024)
    {
        %x = mFloor((3064 + %x) * 0.25);
        if (%y < -1024)
        {
            
            %y = mFloor((3064 + %y) * 0.25);
            %t = 442;
        }
        else
        {
            if (%y < 1020)
            {
                %y = mFloor((1020 + %y) * 0.25);
                %t = 445;
            }
            else
            {
                %y = mFloor((%y - 1024) * 0.25);
                %t = 448;
            }
        }
    }
    else
    {
        if (%x < 1020)
        {
            %x = mFloor((1020 + %x) * 0.25);
            if (%y < -1024)
            {
                %y = mFloor((3064 + %y) * 0.25);
                %t = 443;
            }
            else
            {
                if (%y < 1020)
                {
                    %y = mFloor((1020 + %y) * 0.25);
                    %t = 446;
                }
                else
                {
                    %y = mFloor((%y - 1024) * 0.25);
                    %t = 449;
                }
            }
        }
        else
        {
            %x = mFloor((%x - 1024) * 0.25);
            if (%y < -1024)
            {
                %y = mFloor((3064 + %y) * 0.25);
                %t = 444;
            }
            else
            {
                if (%y < 1020)
                {
                    %y = mFloor((1020 + %y) * 0.25);
                    %t = 447;
                }
                else
                {
                    %y = mFloor((%y - 1024) * 0.25);
                    %t = 450;
                }
            }
        }
    }
    return ((%t << 18) | (%y << 9)) | %x;
  }
  function LiFxUtility::getPositionFromGeoID(%geoID) {
    %t = %geoID >> 18;
    %y = (%geoID >> 9) & 511;
    %x = %geoID & 511;

    if (%t >= 442 && %t <= 450) {
        %tOffset = %t - 442;
        %yOffset = %y * 4;
        %xOffset = %x * 4;

        switch(%t)
        {
          case 442:
            %xPosition = %xOffset - 3064; 
            %yPosition = %yOffset - 3064; 
          case 443:
            %xPosition = %xOffset - 1020; 
            %yPosition = %yOffset - 3064; 
          case 444:
            %xPosition = %xOffset + 1024; 
            %yPosition = %yOffset - 3064; 
          case 445:
            %xPosition = %xOffset - 3064; 
            %yPosition = %yOffset - 1020; 
          case 446:
            %xPosition = %xOffset - 1020; 
            %yPosition = %yOffset - 1020; 
          case 447:
            %xPosition = %xOffset + 1024; 
            %yPosition = %yOffset - 1020; 
          case 448:
            %xPosition = %xOffset - 3064; 
            %yPosition = %yOffset + 1024; 
          case 449:
            %xPosition = %xOffset - 1020; 
            %yPosition = %yOffset + 1024; 
          case 450:
            %xPosition = %xOffset + 1024;
            %yPosition = %yOffset + 1024;
        }
        %startPoint = %xPosition SPC %yPosition SPC 1500; // Start the raycast at a high point above the terrain
        %endPoint = %xPosition SPC %yPosition SPC -1500;   // Extend the raycast downward
        %rayMask =  1 << 2;
        %rayResult = containerRayCast(%startPoint, %endPoint, %rayMask);
        if (%rayResult) {
            %hitPoint = getWords(%rayResult, 1, 3); // Get the point of intersection
            %zPosition = getWord(%hitPoint, 2);     // Get the Z position (height) of the terrain
        } else {
            // No intersection, handle accordingly
            %xPosition = -1;
            %yPosition = -1;
            %zPosition = -1;
        }
    } else {
        // Handle invalid GeoID or undefined t values
        %xPosition = -1;
        %yPosition = -1;
        %zPosition = -1;
    }

    return %xPosition SPC %yPosition SPC %zPosition;
  }
};
