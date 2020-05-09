import json
import requests

original_leaderboard_public_code = input("Original leaderboard public code: ")
new_leaderboard_private_code = input("New leaderboard private code: ")

print (original_leaderboard_public_code)
print (new_leaderboard_private_code)

data = requests.get('http://dreamlo.com/lb/'+original_leaderboard_public_code+'/json')
entries = data['dreamlo']['leaderboard']['entry']

for entry in entries:
	name = entry['name'].replace(' (VR)', '')
	score = entry['score']
	text = '' 
	if not entry['text']:
		ship = 'Rescate'
		vr = False
		if ('VR' in entry['name']):
			vr = True
		text = ship+'|'+str(vr)
	else:
		text = entry['text']

	date = entry['date']

	url = ('http://dreamlo.com/lb/{}/add/{}/{}/0/{}'.format(new_leaderboard_private_code, name, score, text)).replace(' ', '%20')
	print (url)
	resp = requests.get(url)
	print(resp)
