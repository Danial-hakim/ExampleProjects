import anvil.tables as tables
import anvil.tables.query as q
from anvil.tables import app_tables
import anvil.server
import datetime

@anvil.server.http_endpoint("/credentials", methods=["POST"])
def put_credential(**q):
  the_username = anvil.server.request.body_json["username"]
  the_password = anvil.server.request.body_json["password"]
  row = app_tables.credentials.add_row(
      Username =the_username,
      Password =the_password,
  )

@anvil.server.http_endpoint("/checkusernameandpassword", methods=["POST"])
def check_username(**q):
  the_username = anvil.server.request.body_json["username"]
  the_password = anvil.server.request.body_json["password"]
  #x = app_tables.credentials.search(q.any_of(Username = the_username))
  #if x == the_username:
  #    return "CanLogIn"
  
  
  #user_details = {'Username':the_username, 'Password': the_password}
  #if app_tables.credentials.search(**user_details):
  #  return "CanLogIn"
  if [[row['Username']] for row in app_tables.credentials.search(Username = the_username)]:
    if [row['Password'] for row in app_tables.credentials.search(Password = the_password)]:
      return "CanLogIn"
 
  return "Cant log in"


@anvil.server.http_endpoint("/checkusername", methods=["POST"])
def check_username(**q):
  the_username = anvil.server.request.body_json["username"]
  numberTest = 0

  #if numberTest == 0:
 #       return "false"
 # elif the_username != [[row['Username']] for row in app_tables.credentials.search()]:
 #       return "true"
  if [[row['Username']] for row in app_tables.credentials.search(Username = the_username)]:
      return "Taken"
 
  return "NotTaken"

  
@anvil.server.http_endpoint("/leaderboard", methods=["POST"])
def put_leaderboard(**q):
  the_username = anvil.server.request.body_json["username"]
  the_score = anvil.server.request.body_json["score"]
  row = app_tables.leaderboard.add_row(
      username =the_username,
      score =the_score,
  )
  anvil.server.call('get_highestScores')

@anvil.server.http_endpoint("/playtestdata", methods=["POST"])
def put_metric(**q):
  the_id = anvil.server.request.body_json["playerID"]
  the_role = anvil.server.request.body_json["role"]
  the_distanceTravelled = anvil.server.request.body_json["distanceTravelled"]
  the_obstaclesAvoided = anvil.server.request.body_json["obstaclesAvoided"]
  the_obstaclesHit = anvil.server.request.body_json["obstaclesHit"]
  the_obstaclesPlaced = anvil.server.request.body_json["obstaclesPlaced"]
  the_score = anvil.server.request.body_json["score"]
  the_time = anvil.server.request.body_json["timePlayed"]
  
  row = app_tables.feedback.add_row(
    PlayerID=the_id,
    Role=the_role,
    DistanceTravelled=the_distanceTravelled,
    ObstaclesAvoided=the_obstaclesAvoided,
    ObstaclesHit=the_obstaclesHit,
    ObstaclesPlaced=the_obstaclesPlaced,
    Score=the_score,
    TimePlayed=the_time
  )
  #return {"playerID" : row.get_PlayerID()}

@anvil.server.http_endpoint("/usernames", methods=["GET"])
def get_usernames(**q):
  return [[row['Username']] 
            for row in app_tables.credentials.search()]

@anvil.server.http_endpoint("/passwords")
def get_passwords(**q):
  return [[row['Password']] 
            for row in app_tables.credentials.search()]

@anvil.server.http_endpoint("/metrics")
def get_metrics(**q):
  return [[row['PlayerID'],row['Role'],
            row['DistanceTravelled'],row['ObstaclesAvoided'],row['ObstaclesHit'],row['ObstaclesPlaced'],row['Score'],row['TimePlayed']] 
            for row in app_tables.metrics.search()]

@anvil.server.callable
def get_data():
  return app_tables.feedback.search(tables.order_by('PlayerID', ascending=True))

@anvil.server.callable
def get_highestScores():

  rowNumber = 0

  highestScores = [[row['score'],row['username']]         
    for row in app_tables.leaderboard.search()]

  highestScores.sort(reverse =True)
  
  for row in app_tables.leaderboard.search():
    rowNumber +=1
    #print(rowNumber)
    row.update(number=rowNumber)

  increment =0;
  for x in range(len(highestScores)):
    #print(highestScores[x][0])
    increment +=1
    target_row = app_tables.leaderboard.get(number =increment)
    target_row.update(score = highestScores[x][0])
    target_row.update(username = highestScores[x][1])
    
  row = app_tables.leaderboard.get(number = 23)
  if row is not None:
    row.delete()
  
  return highestScores

@anvil.server.http_endpoint("/getleaderboard", methods=["GET"])
def get_leaderboard(**q):
  
  highestScores = [[row['username'],row['score']]         
    for row in app_tables.leaderboard.search()]

  return highestScores