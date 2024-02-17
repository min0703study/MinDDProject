import csv
import json
import os

data = []
for full_filename in os.listdir('./csv'):
    filename = os.path.splitext(full_filename)[0]

    if (os.path.splitext(full_filename)[1] == ".meta") :
        continue
    
    if (full_filename.startswith(".")) :
        continue

    with open('csv/' + full_filename, 'r', encoding='euc-kr') as csv_file:
        try:
            reader = csv.DictReader(csv_file)
            data = list(reader)
            print('성공 : ' + full_filename)
            print('=======================')
        except Exception as e:
            print('오류 발생 : ' + full_filename)
            print(e)
            print('=======================')
            

    with open('json/' + filename + '.json', 'w',  encoding='UTF-8-sig') as json_file:
        json.dump(data, json_file, ensure_ascii=False, indent = 4)
