# kafka 示例

## client
   想自己弄一个没有成功

## 参考示例，分两个项目
### ConsumerTest
   消息接受方（消费者）

### ProducerTest
   消息发送方（生产者）	

### kafaka docker 说明
相关文档 [link](https://www.mrjamiebowman.com/software-development/getting-started-with-landoop-kafka-on-docker-locally/)

	-使用的docker images: `docker pull landoop/fast-data-dev`
	-启动 `1
docker run -d --name landoopkafka -p 2181:2181 -p 3030:3030 -p 8081:8081 -p 8082:8082 -p 8083:8083 -p 9092:9092 -e ADV_HOST=127.0.0.1 landoop/fast-data-dev`
	- Landoop’s Kafka UI [link](http://127.0.0.1:3030/)
