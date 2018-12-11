from bs4 import BeautifulSoup
import re
from requests import get

response = get("https://www.name-generator.org.uk/?i=8pil4gx")
html = response.content
#html = open('RandomNames.html').read()
soup = BeautifulSoup(html, 'html.parser')
output_list = []

for i, div in enumerate(soup.find_all('div', class_='name_heading')):
    output_list.append(div.contents)

with open('./Data/Servers.txt', 'w') as f:
    for line in output_list:
        line = re.sub('[\[\]\"\']', '', str(line))
        f.write("%s\n" % line)