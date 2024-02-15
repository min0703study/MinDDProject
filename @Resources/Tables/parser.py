import csv
import json
import os

data = []
for full_filename in os.listdir('./csv'):
    filename = os.path.splitext(full_filename)[0]

    if (os.path.splitext(full_filename)[1] == ".meta") :
        continue
    
    with open('csv/' + full_filename, 'r') as csv_file:
        reader = csv.DictReader(csv_file)
        data = list(reader)

    with open('json/' + filename + '.json', 'w',  encoding='UTF-8-sig') as json_file:
        json.dump(data, json_file, ensure_ascii=False, indent = 4)

print('작업 완료')
