# kafka ʾ��

## client
   ���Լ�Ūһ��û�гɹ�

## �ο�ʾ������������Ŀ
### ConsumerTest
   ��Ϣ���ܷ��������ߣ�

### ProducerTest
   ��Ϣ���ͷ��������ߣ�	

### kafaka docker ˵��
����ĵ� [link](https://www.mrjamiebowman.com/software-development/getting-started-with-landoop-kafka-on-docker-locally/)

	-ʹ�õ�docker images: `docker pull landoop/fast-data-dev`
	-���� `1
docker run -d --name landoopkafka -p 2181:2181 -p 3030:3030 -p 8081:8081 -p 8082:8082 -p 8083:8083 -p 9092:9092 -e ADV_HOST=127.0.0.1 landoop/fast-data-dev`
	- Landoop��s Kafka UI [link](http://127.0.0.1:3030/)
